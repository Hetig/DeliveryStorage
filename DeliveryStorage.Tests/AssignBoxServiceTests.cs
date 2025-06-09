using AutoMapper;
using DeliveryStorage.Database.Entities;
using DeliveryStorage.Database.Interfaces;
using DeliveryStorage.Domain.Interfaces;
using DeliveryStorage.Domain.Models;
using DeliveryStorage.Domain.Services;
using Moq;
using System.Linq.Expressions;
using Xunit;

namespace DeliveryStorage.Tests
{
	public class AssignBoxServiceTests
	{
		private readonly Mock<IBaseRepository<BoxDb>> _mockBoxRepository;
		private readonly Mock<IBaseRepository<PalletDb>> _mockPalletRepository;
		private readonly Mock<IMapper> _mockMapper;
		private readonly AssignBoxService _service;

		public AssignBoxServiceTests()
		{
			_mockBoxRepository = new Mock<IBaseRepository<BoxDb>>();
			_mockPalletRepository = new Mock<IBaseRepository<PalletDb>>();
			_mockMapper = new Mock<IMapper>();
			_service = new AssignBoxService(_mockMapper.Object, _mockBoxRepository.Object, _mockPalletRepository.Object);
		}

		[Fact]
		public async Task AssignBoxToPallet_WhenPalletNotFound_ReturnsNull()
		{
			var palletId = Guid.NewGuid();
			var boxIds = new List<Guid> { Guid.NewGuid() };

			_mockPalletRepository.Setup(x => x.GetByIdAsync(
				It.IsAny<Expression<Func<PalletDb, bool>>>(),
				It.IsAny<Expression<Func<PalletDb, object>>>()))
				.ReturnsAsync((PalletDb)null);

			var result = await _service.AssignBoxToPallet(palletId, boxIds);

			Assert.Null(result);
			_mockPalletRepository.Verify(x => x.GetByIdAsync(It.IsAny<Expression<Func<PalletDb, bool>>>(), It.IsAny<Expression<Func<PalletDb, object>>>()), Times.Once);
			_mockBoxRepository.Verify(x => x.GetByIdAsync(It.IsAny<Expression<Func<BoxDb, bool>>>()), Times.Never);
		}

		[Fact]
		public async Task AssignBoxToPallet_WhenBoxTooLarge_ReturnsNull()
		{
			var palletId = Guid.NewGuid();
			var boxId = Guid.NewGuid();
			var boxIds = new List<Guid> { boxId };
			var pallet = new PalletDb { Id = palletId, Width = 10, Height = 10, Boxes = new List<BoxDb>() };
			var box = new BoxDb { Id = boxId, Width = 15, Height = 15 };

			_mockPalletRepository.Setup(x => x.GetByIdAsync(
				It.IsAny<Expression<Func<PalletDb, bool>>>(),
				It.IsAny<Expression<Func<PalletDb, object>>>()))
				.ReturnsAsync(pallet);

			_mockBoxRepository.Setup(x => x.GetByIdAsync(It.IsAny<Expression<Func<BoxDb, bool>>>()))
				.ReturnsAsync(box);

			var result = await _service.AssignBoxToPallet(palletId, boxIds);

			Assert.Null(result);
		}

		[Fact]
		public async Task AssignBoxToPallet_WhenValidData_ReturnsMappedPallet()
		{
			var palletId = Guid.NewGuid();
			var boxId = Guid.NewGuid();
			var boxIds = new List<Guid> { boxId };
			var pallet = new PalletDb { Id = palletId, Width = 10, Height = 10, Boxes = new List<BoxDb>() };
			var box = new BoxDb { Id = boxId, Width = 5, Height = 5 };
			var expectedPallet = new Pallet { Id = palletId };

			_mockPalletRepository.Setup(x => x.GetByIdAsync(
				It.IsAny<Expression<Func<PalletDb, bool>>>(),
				It.IsAny<Expression<Func<PalletDb, object>>>()))
				.ReturnsAsync(pallet);

			_mockBoxRepository.Setup(x => x.GetByIdAsync(It.IsAny<Expression<Func<BoxDb, bool>>>()))
				.ReturnsAsync(box);

			_mockPalletRepository.Setup(x => x.UpdateAsync(It.IsAny<PalletDb>()))
				.ReturnsAsync(pallet);

			_mockMapper.Setup(x => x.Map<Pallet>(pallet))
				.Returns(expectedPallet);

			var result = await _service.AssignBoxToPallet(palletId, boxIds);

			Assert.NotNull(result);
			Assert.Equal(expectedPallet, result);
			_mockPalletRepository.Verify(x => x.UpdateAsync(It.Is<PalletDb>(p =>
				p.Id == palletId &&
				p.Boxes.Count == 1 &&
				p.Boxes.First().Id == boxId)), Times.Once);
			_mockMapper.Verify(x => x.Map<Pallet>(pallet), Times.Once);
		}

		[Fact]
		public async Task AssignBoxToPallet_WithMultipleBoxes_WhenOneBoxTooLarge_ReturnsNull()
		{
			var palletId = Guid.NewGuid();
			var validBoxId = Guid.NewGuid();
			var invalidBoxId = Guid.NewGuid();
			var boxIds = new List<Guid> { validBoxId, invalidBoxId };
			var pallet = new PalletDb { Id = palletId, Width = 10, Height = 10, Boxes = new List<BoxDb>() };
			var validBox = new BoxDb { Id = validBoxId, Width = 5, Height = 5 };
			var invalidBox = new BoxDb { Id = invalidBoxId, Width = 15, Height = 15 };

			_mockPalletRepository.Setup(x => x.GetByIdAsync(
				It.IsAny<Expression<Func<PalletDb, bool>>>(),
				It.IsAny<Expression<Func<PalletDb, object>>>()))
				.ReturnsAsync(pallet);

			_mockBoxRepository.SetupSequence(x => x.GetByIdAsync(It.IsAny<Expression<Func<BoxDb, bool>>>()))
				.ReturnsAsync(validBox)
				.ReturnsAsync(invalidBox);

			var result = await _service.AssignBoxToPallet(palletId, boxIds);

			Assert.Null(result);
			_mockPalletRepository.Verify(x => x.UpdateAsync(It.IsAny<PalletDb>()), Times.Never);
		}
	}
}