using Harvest.Net.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Harvest.Net.Tests
{
    public class TaskFacts : FactBase, IDisposable
    {
        Models.ProjectTask _todelete = null;
        #region Standard Api
        [Fact]
        public void ListTasks_Returns()
        {
            var list = Api.ListTasks();

            Assert.True(list != null, "Result list is null.");
            Assert.NotEqual(0, list.First().Id);
        }

        [Fact]
        public void Task_ReturnsTask()
        {
            var task = Api.Task(GetTestId(TestId.TaskId));

            Assert.NotNull(task);
            Assert.Equal("Admin", task.Name);
        }

        [Fact]
        public void DeleteTask_ReturnsTrue()
        {
            var task = Api.CreateTask("Delete Task");

            var result = Api.DeleteTask(task.Id);

            Assert.Equal(true, result);
        }

        [Fact]
        public void CreateTask_ReturnsANewTask()
        {
            _todelete = Api.CreateTask("Create Task");

            Assert.Equal("Create Task", _todelete.Name);
        }

        [Fact]
        public void UpdateTask_UpdatesOnlyChangedValues()
        {
            _todelete = Api.CreateTask("Update Task", billableByDefault: true, isDefault: false);

            var updated = Api.UpdateTask(_todelete.Id, name: "Updated Task", isDefault: true);

            // stuff changed
            Assert.NotEqual(_todelete.Name, updated.Name);
            Assert.Equal("Updated Task", updated.Name);
            Assert.NotEqual(_todelete.IsDefault, updated.IsDefault);
            Assert.Equal(true, updated.IsDefault);

            // stuff didn't change
            Assert.Equal(_todelete.BillableByDefault, updated.BillableByDefault);
            Assert.Equal(_todelete.DefaultHourlyRate, updated.DefaultHourlyRate);
        }

        #endregion

        #region Async Api
        [Fact]
        public async Task ListTasksAsync_Returns()
        {
            var list = await Api.ListTasksAsync();

            Assert.True(list != null, "Result list is null.");
            Assert.NotEqual(0, list.First().Id);
        }

        [Fact]
        public async Task TaskAsync_ReturnsTask()
        {
            var task = await Api.TaskAsync(GetTestId(TestId.TaskId));

            Assert.NotNull(task);
            Assert.Equal("Admin", task.Name);
        }

        [Fact]
        public async Task DeleteTaskAsync_ReturnsTrue()
        {
            var task = await Api.CreateTaskAsync("Delete Task");

            var result = await Api.DeleteTaskAsync(task.Id);

            Assert.Equal(true, result);
        }

        [Fact]
        public async Task CreateTaskAsync_ReturnsANewTask()
        {
            _todelete = await Api.CreateTaskAsync("Create Task");

            Assert.Equal("Create Task", _todelete.Name);
        }

        [Fact]
        public async Task UpdateTaskAsync_UpdatesOnlyChangedValues()
        {
            _todelete = await Api.CreateTaskAsync("Update Task", billableByDefault: true, isDefault: false);

            var updated = await Api.UpdateTaskAsync(_todelete.Id, name: "Updated Task", isDefault: true);

            // stuff changed
            Assert.NotEqual(_todelete.Name, updated.Name);
            Assert.Equal("Updated Task", updated.Name);
            Assert.NotEqual(_todelete.IsDefault, updated.IsDefault);
            Assert.Equal(true, updated.IsDefault);

            // stuff didn't change
            Assert.Equal(_todelete.BillableByDefault, updated.BillableByDefault);
            Assert.Equal(_todelete.DefaultHourlyRate, updated.DefaultHourlyRate);
        }
        #endregion
        
        public void Dispose()
        {
            if (_todelete != null)
                Api.DeleteTask(_todelete.Id);
        }
    }
}
