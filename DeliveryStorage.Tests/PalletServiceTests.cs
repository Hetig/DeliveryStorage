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
	public class PalletServiceTests
	{
		private readonly Mock<IMapper> _mockMapper;
		private readonly Mock<IBaseRepository<PalletDb>> _mockPalletRepository;
		private readonly PalletService _service;

		public PalletServiceTests()
		{
			_mockMapper = new Mock<IMapper>();
			_mockPalletRepository = new Mock<IBaseRepository<PalletDb>>();
			_service = new PalletService(_mockMapper.Object, _mockPalletRepository.Object);
		}


		[Fact]
		public async Task GetByIdAsync_WhenPalletNotExists_ReturnsNull()
		{
			var palletId = Guid.NewGuid();

			_mockPalletRepository.Setup(x => x.GetByIdAsync(
				It.IsAny<Expression<Func<PalletDb, bool>>>(),
				It.IsAny<Expression<Func<PalletDb, object>>>()))
				.ReturnsAsync((PalletDb)null);

			var result = await _service.GetByIdAsync(palletId);

			Assert.Null(result);
		}

		[Fact]
		public async Task AddAsync_ValidPallet_ReturnsMappedPallet()
		{
			var pallet = new Pallet();
			var palletDb = new PalletDb();
			var createdPalletDb = new PalletDb();
			var expectedPallet = new Pallet();

			_mockMapper.Setup(x => x.Map<PalletDb>(pallet))
				.Returns(palletDb);
			_mockPalletRepository.Setup(x => x.AddAsync(palletDb))
				.ReturnsAsync(createdPalletDb);
			_mockMapper.Setup(x => x.Map<Pallet>(createdPalletDb))
				.Returns(expectedPallet);

			var result = await _service.AddAsync(pallet);

			Assert.Equal(expectedPallet, result);
			_mockMapper.Verify(x => x.Map<PalletDb>(pallet), Times.Once);
			_mockPalletRepository.Verify(x => x.AddAsync(palletDb), Times.Once);
			_mockMapper.Verify(x => x.Map<Pallet>(createdPalletDb), Times.Once);
		}

		[Fact]
		public async Task UpdateAsync_WhenPalletExists_UpdatesAndReturnsMappedPallet()
		{
			var palletId = Guid.NewGuid();
			var boxId = Guid.NewGuid();
			var pallet = new Pallet
			{
				Id = palletId,
				Boxes = new List<Box> { new Box { Id = boxId } }
			};
			var existingPalletDb = new PalletDb { Id = palletId };
			var updatedPalletDb = new PalletDb { Id = palletId };
			var expectedPallet = new Pallet { Id = palletId };
			var boxDbs = new List<BoxDb> { new BoxDb { Id = boxId } };

			_mockPalletRepository.Setup(x => x.ExistsAsync(palletId))
				.ReturnsAsync(true);
			_mockPalletRepository.Setup(x => x.GetByIdAsync(It.IsAny<Expression<Func<PalletDb, bool>>>()))
				.ReturnsAsync(existingPalletDb);
			_mockMapper.Setup(x => x.Map<List<BoxDb>>(pallet.Boxes))
				.Returns(boxDbs);
			_mockPalletRepository.Setup(x => x.UpdateAsync(It.IsAny<PalletDb>()))
				.ReturnsAsync(updatedPalletDb);
			_mockMapper.Setup(x => x.Map<Pallet>(updatedPalletDb))
				.Returns(expectedPallet);

			var result = await _service.UpdateAsync(pallet);

			Assert.Equal(expectedPallet, result);
			Assert.Equal(pallet.Height, existingPalletDb.Height);
			Assert.Equal(pallet.Width, existingPalletDb.Width);
			Assert.Equal(pallet.Depth, existingPalletDb.Depth);
			Assert.Equal(boxDbs, existingPalletDb.Boxes);
			_mockPalletRepository.Verify(x => x.UpdateAsync(existingPalletDb), Times.Once);
		}

		[Fact]
		public async Task UpdateAsync_WhenPalletNotExists_ReturnsNull()
		{
			var palletId = Guid.NewGuid();
			var pallet = new Pallet { Id = palletId };

			_mockPalletRepository.Setup(x => x.ExistsAsync(palletId))
				.ReturnsAsync(false);

			var result = await _service.UpdateAsync(pallet);

			Assert.Null(result);
			_mockPalletRepository.Verify(x => x.GetByIdAsync(It.IsAny<Expression<Func<PalletDb, bool>>>()), Times.Never);
			_mockPalletRepository.Verify(x => x.UpdateAsync(It.IsAny<PalletDb>()), Times.Never);
		}

		[Fact]
		public async Task DeleteAsync_WhenPalletExists_ReturnsTrue()
		{
			var palletId = Guid.NewGuid();
			var palletDb = new PalletDb { Id = palletId };

			_mockPalletRepository.Setup(x => x.ExistsAsync(palletId))
				.ReturnsAsync(true);
			_mockPalletRepository.Setup(x => x.GetByIdAsync(It.IsAny<Expression<Func<PalletDb, bool>>>()))
				.ReturnsAsync(palletDb);

			var result = await _service.DeleteAsync(palletId);

			Assert.True(result);
			_mockPalletRepository.Verify(x => x.DeleteAsync(palletDb), Times.Once);
		}

		[Fact]
		public async Task DeleteAsync_WhenPalletNotExists_ReturnsFalse()
		{
			var palletId = Guid.NewGuid();

			_mockPalletRepository.Setup(x => x.ExistsAsync(palletId))
				.ReturnsAsync(false);

			var result = await _service.DeleteAsync(palletId);

			Assert.False(result);
			_mockPalletRepository.Verify(x => x.DeleteAsync(It.IsAny<PalletDb>()), Times.Never);
		}
	}
}