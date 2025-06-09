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
	public class BoxServiceTests
	{
		private readonly Mock<IMapper> _mockMapper;
		private readonly Mock<IBaseRepository<BoxDb>> _mockBoxRepository;
		private readonly BoxService _service;

		public BoxServiceTests()
		{
			_mockMapper = new Mock<IMapper>();
			_mockBoxRepository = new Mock<IBaseRepository<BoxDb>>();
			_service = new BoxService(_mockMapper.Object, _mockBoxRepository.Object);
		}

		[Fact]
		public async Task GetByIdAsync_WhenBoxNotExists_ReturnsNull()
		{
			var boxId = Guid.NewGuid();

			_mockBoxRepository.Setup(x => x.GetByIdAsync(It.IsAny<Expression<Func<BoxDb, bool>>>()))
				.ReturnsAsync((BoxDb)null);

			var result = await _service.GetByIdAsync(boxId);

			Assert.Null(result);
		}

		[Fact]
		public async Task AddAsync_ValidBox_ReturnsMappedBox()
		{
			var box = new Box();
			var boxDb = new BoxDb();
			var createdBoxDb = new BoxDb();
			var expectedBox = new Box();

			_mockMapper.Setup(x => x.Map<BoxDb>(box))
				.Returns(boxDb);
			_mockBoxRepository.Setup(x => x.AddAsync(boxDb))
				.ReturnsAsync(createdBoxDb);
			_mockMapper.Setup(x => x.Map<Box>(createdBoxDb))
				.Returns(expectedBox);

			var result = await _service.AddAsync(box);

			Assert.Equal(expectedBox, result);
			_mockMapper.Verify(x => x.Map<BoxDb>(box), Times.Once);
			_mockBoxRepository.Verify(x => x.AddAsync(boxDb), Times.Once);
			_mockMapper.Verify(x => x.Map<Box>(createdBoxDb), Times.Once);
		}

		[Fact]
		public async Task UpdateAsync_WhenBoxExists_UpdatesAndReturnsMappedBox()
		{
			var boxId = Guid.NewGuid();
			var box = new Box { Id = boxId };
			var existingBoxDb = new BoxDb { Id = boxId };
			var updatedBoxDb = new BoxDb { Id = boxId };
			var expectedBox = new Box { Id = boxId };

			_mockBoxRepository.Setup(x => x.ExistsAsync(boxId))
				.ReturnsAsync(true);
			_mockBoxRepository.Setup(x => x.GetByIdAsync(It.IsAny<Expression<Func<BoxDb, bool>>>()))
				.ReturnsAsync(existingBoxDb);
			_mockBoxRepository.Setup(x => x.UpdateAsync(It.IsAny<BoxDb>()))
				.ReturnsAsync(updatedBoxDb);
			_mockMapper.Setup(x => x.Map<Box>(updatedBoxDb))
				.Returns(expectedBox);

			var result = await _service.UpdateAsync(box);

			Assert.Equal(expectedBox, result);
			Assert.Equal(box.Height, existingBoxDb.Height);
			Assert.Equal(box.Width, existingBoxDb.Width);
			Assert.Equal(box.Weight, existingBoxDb.Weight);
			Assert.Equal(box.ProductionDate, existingBoxDb.ProductionDate);
			Assert.Equal(box.Depth, existingBoxDb.Depth);
			_mockBoxRepository.Verify(x => x.UpdateAsync(existingBoxDb), Times.Once);
		}

		[Fact]
		public async Task UpdateAsync_WhenBoxNotExists_ReturnsNull()
		{
			var boxId = Guid.NewGuid();
			var box = new Box { Id = boxId };

			_mockBoxRepository.Setup(x => x.ExistsAsync(boxId))
				.ReturnsAsync(false);

			var result = await _service.UpdateAsync(box);

			Assert.Null(result);
			_mockBoxRepository.Verify(x => x.GetByIdAsync(It.IsAny<Expression<Func<BoxDb, bool>>>()), Times.Never);
			_mockBoxRepository.Verify(x => x.UpdateAsync(It.IsAny<BoxDb>()), Times.Never);
		}

		[Fact]
		public async Task DeleteAsync_WhenBoxExists_ReturnsTrue()
		{
			var boxId = Guid.NewGuid();
			var boxDb = new BoxDb { Id = boxId };

			_mockBoxRepository.Setup(x => x.ExistsAsync(boxId))
				.ReturnsAsync(true);
			_mockBoxRepository.Setup(x => x.GetByIdAsync(It.IsAny<Expression<Func<BoxDb, bool>>>()))
				.ReturnsAsync(boxDb);

			var result = await _service.DeleteAsync(boxId);

			Assert.True(result);
			_mockBoxRepository.Verify(x => x.DeleteAsync(boxDb), Times.Once);
		}

		[Fact]
		public async Task DeleteAsync_WhenBoxNotExists_ReturnsFalse()
		{
			var boxId = Guid.NewGuid();

			_mockBoxRepository.Setup(x => x.ExistsAsync(boxId))
				.ReturnsAsync(false);

			var result = await _service.DeleteAsync(boxId);

			Assert.False(result);
			_mockBoxRepository.Verify(x => x.DeleteAsync(It.IsAny<BoxDb>()), Times.Never);
		}

		[Fact]
		public async Task GetAllAsync_ReturnsMappedBoxes()
		{
			var boxDbs = new List<BoxDb> { new BoxDb(), new BoxDb() };
			var expectedBoxes = new List<Box> { new Box(), new Box() };

			_mockBoxRepository.Setup(x => x.GetAllAsync())
				.ReturnsAsync(boxDbs);
			_mockMapper.Setup(x => x.Map<List<Box>>(boxDbs))
				.Returns(expectedBoxes);

			var result = await _service.GetAllAsync();

			Assert.Equal(expectedBoxes, result);
			_mockBoxRepository.Verify(x => x.GetAllAsync(), Times.Once);
			_mockMapper.Verify(x => x.Map<List<Box>>(boxDbs), Times.Once);
		}
	}
}