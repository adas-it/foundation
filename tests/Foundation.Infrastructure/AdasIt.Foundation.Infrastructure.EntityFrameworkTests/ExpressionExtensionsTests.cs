using AdasIt.Foundation.Infrastructure.EntityFramework;
using System.Linq.Expressions;

namespace AdasIt.Foundation.Infrastructure.EntityFrameworkTests
{
    public class ExpressionExtensionsTests
    {
        private class TestEntity
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        [Fact]
        public void Or_ShouldCombineTwoTrueExpressions_ReturnTrue()
        {
            // Arrange
            Expression<Func<TestEntity, bool>> left = x => x.Id == 1;
            Expression<Func<TestEntity, bool>> right = x => x.Name == "Test";
            var entity = new TestEntity { Id = 1, Name = "Test" };

            // Act
            var combined = left.Or(right);
            var result = combined.Compile()(entity);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Or_ShouldCombineOneTrueOneFalseExpressions_ReturnTrue()
        {
            // Arrange
            Expression<Func<TestEntity, bool>> left = x => x.Id == 1;
            Expression<Func<TestEntity, bool>> right = x => x.Name == "Wrong";
            var entity = new TestEntity { Id = 1, Name = "Test" };

            // Act
            var combined = left.Or(right);
            var result = combined.Compile()(entity);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Or_ShouldCombineTwoFalseExpressions_ReturnFalse()
        {
            // Arrange
            Expression<Func<TestEntity, bool>> left = x => x.Id == 2;
            Expression<Func<TestEntity, bool>> right = x => x.Name == "Wrong";
            var entity = new TestEntity { Id = 1, Name = "Test" };

            // Act
            var combined = left.Or(right);
            var result = combined.Compile()(entity);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Or_ShouldHandleComplexExpressions()
        {
            // Arrange
            Expression<Func<TestEntity, bool>> left = x => x.Id > 0 && x.Name.StartsWith("T");
            Expression<Func<TestEntity, bool>> right = x => x.Id == 5 || x.Name == "Other";
            var entity = new TestEntity { Id = 1, Name = "Test" };

            // Act
            var combined = left.Or(right);
            var result = combined.Compile()(entity);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Or_ShouldReplaceParametersCorrectly()
        {
            // Arrange
            Expression<Func<TestEntity, bool>> left = y => y.Id == 1;
            Expression<Func<TestEntity, bool>> right = z => z.Name == "Test";
            var entity = new TestEntity { Id = 1, Name = "Test" };

            // Act
            var combined = left.Or(right);
            var result = combined.Compile()(entity);

            // Assert
            Assert.True(result);
            Assert.Equal("x", combined.Parameters[0].Name);
        }
    }
}
