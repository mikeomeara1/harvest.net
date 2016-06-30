using Harvest.Net.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Harvest.Net.Tests
{
    public class ProjectFacts : FactBase, IDisposable
    {
        Project _todelete = null;
        #region Standard Api
        [Fact]
        public void ListProjects_Returns()
        {
            var list = Api.ListProjects();

            Assert.True(list != null, "Result list is null.");
            Assert.NotEqual(0, list.First().Id);
        }

        [Fact]
        public void ListProjects_WithBudgetOn_Returns()
        {
            var project = Api.Project(GetTestId(TestId.ProjectId));

            Api.UpdateProject(project.Id, project.ClientId, budgetBy: BudgetMethod.ProjectCost, costBudget: 1, costBudgetIncludeExpenses: false);

            var list = Api.ListProjects();

            var updated = list.First(p => p.Id == GetTestId(TestId.ProjectId));

            Assert.Equal(BudgetMethod.ProjectCost, updated.BudgetBy);
            Assert.Equal(1m, updated.CostBudget);

            Api.UpdateProject(project.Id, project.ClientId, budgetBy: BudgetMethod.None);
        }

        [Fact]
        public void ListProjects_WithBillingMethod_Returns()
        {
            var project = Api.Project(GetTestId(TestId.ProjectId));

            Api.UpdateProject(project.Id, project.ClientId, billBy: BillingMethod.People);

            var list = Api.ListProjects();

            var updated = list.First(p => p.Id == GetTestId(TestId.ProjectId));

            Assert.Equal(BillingMethod.People, updated.BillBy);

            Api.UpdateProject(project.Id, project.ClientId, billBy: BillingMethod.None);
        }

        [Fact]
        public void Project_ReturnsProject()
        {
            var Project = Api.Project(GetTestId(TestId.ProjectId));

            Assert.NotNull(Project);
            Assert.Equal("Test DO NOT DELETE", Project.Name);
        }

        [Fact(Skip = "Fails because the account has hit the max projects limit")]
        public void DeleteProject_ReturnsTrue()
        {
            var Project = Api.CreateProject("Test Delete Project", GetTestId(TestId.ClientId));

            var result = Api.DeleteProject(Project.Id);

            Assert.Equal(true, result);
        }

        [Fact(Skip = "Fails because the account has hit the max projects limit")]
        public void CreateProject_ReturnsANewProject()
        {
            _todelete = Api.CreateProject("Test Create Project", GetTestId(TestId.ClientId));

            Assert.Equal("Test Create Project", _todelete.Name);
            Assert.Equal(GetTestId(TestId.ClientId), _todelete.ClientId);
        }

        // https://github.com/harvesthq/api/issues/93 for the two tests below

        [Fact(Skip = "Fails because the account has hit the max projects limit")]
        public void ToggleProject_TogglesTheProjectStatus()
        {
            _todelete = Api.CreateProject("Test Toggle Project", GetTestId(TestId.ClientId));

            Assert.Equal(true, _todelete.Active);

            var result = Api.ToggleProject(_todelete.Id);
            var toggled = Api.Project(_todelete.Id);

            Assert.Equal(true, result);
            Assert.Equal(false, toggled.Active);
        }

        [Fact(Skip = "Fails because the account has hit the max projects limit")]
        public void UpdateProject_UpdatesOnlyChangedValues()
        {
            _todelete = Api.CreateProject("Test Update Project", GetTestId(TestId.ClientId));

            var updated = Api.UpdateProject(_todelete.Id, _todelete.ClientId, name: "Test Updated Project", notes: "notes");

            // stuff changed
            Assert.NotEqual(_todelete.Name, updated.Name);
            Assert.Equal("Test Updated Project", updated.Name);
            Assert.NotEqual(_todelete.Notes, updated.Notes);
            Assert.Equal("notes", updated.Notes);

            // stuff didn't change
            Assert.Equal(_todelete.Active, updated.Active);
            Assert.Equal(_todelete.BillBy, updated.BillBy);
            Assert.Equal(_todelete.Budget, updated.Budget);
            Assert.Equal(_todelete.ClientId, updated.ClientId);
        }

        #endregion
        #region Async Api
        [Fact]
        public async Task ListProjectsAsync_Returns()
        {
            var list = await Api.ListProjectsAsync();

            Assert.True(list != null, "Result list is null.");
            Assert.NotEqual(0, list.First().Id);
        }

        [Fact]
        public async Task ListProjects_WithBudgetOnAsync_Returns()
        {
            var project = await Api.ProjectAsync(GetTestId(TestId.ProjectId));

            await Api.UpdateProjectAsync(project.Id, project.ClientId, budgetBy: BudgetMethod.ProjectCost, costBudget: 1, costBudgetIncludeExpenses: false);

            var list = await Api.ListProjectsAsync();

            var updated = list.First(p => p.Id == GetTestId(TestId.ProjectId));

            Assert.Equal(BudgetMethod.ProjectCost, updated.BudgetBy);
            Assert.Equal(1m, updated.CostBudget);

            await Api.UpdateProjectAsync(project.Id, project.ClientId, budgetBy: BudgetMethod.None);
        }

        [Fact]
        public async Task ListProjects_WithBillingMethodAsync_Returns()
        {
            var project = await Api.ProjectAsync(GetTestId(TestId.ProjectId));

            await Api.UpdateProjectAsync(project.Id, project.ClientId, billBy: BillingMethod.People);

            var list = await Api.ListProjectsAsync();

            var updated = list.First(p => p.Id == GetTestId(TestId.ProjectId));

            Assert.Equal(BillingMethod.People, updated.BillBy);

            await Api.UpdateProjectAsync(project.Id, project.ClientId, billBy: BillingMethod.None);
        }

        [Fact]
        public async Task ProjectAsync_ReturnsProject()
        {
            var Project = await Api.ProjectAsync(GetTestId(TestId.ProjectId));

            Assert.NotNull(Project);
            Assert.Equal("Test DO NOT DELETE", Project.Name);
        }

        [Fact(Skip = "Fails because the account has hit the max projects limit")]
        public async Task DeleteProjectAsync_ReturnsTrue()
        {
            var Project = await Api.CreateProjectAsync("Test Delete Project", GetTestId(TestId.ClientId));

            var result = await Api.DeleteProjectAsync(Project.Id);

            Assert.Equal(true, result);
        }

        [Fact(Skip = "Fails because the account has hit the max projects limit")]
        public async Task CreateProjectAsync_ReturnsANewProject()
        {
            _todelete = await Api.CreateProjectAsync("Test Create Project", GetTestId(TestId.ClientId));

            Assert.Equal("Test Create Project", _todelete.Name);
            Assert.Equal(GetTestId(TestId.ClientId), _todelete.ClientId);
        }

        // https://github.com/harvesthq/api/issues/93 for the two tests below

        [Fact(Skip = "Fails because the account has hit the max projects limit")]
        public async Task ToggleProjectAsync_TogglesTheProjectStatus()
        {
            _todelete = await Api.CreateProjectAsync("Test Toggle Project", GetTestId(TestId.ClientId));

            Assert.Equal(true, _todelete.Active);

            var result = await Api.ToggleProjectAsync(_todelete.Id);
            var toggled = await Api.ProjectAsync(_todelete.Id);

            Assert.Equal(true, result);
            Assert.Equal(false, toggled.Active);
        }

        [Fact(Skip = "Fails because the account has hit the max projects limit")]
        public async Task UpdateProjectAsync_UpdatesOnlyChangedValues()
        {
            _todelete = await Api.CreateProjectAsync("Test Update Project", GetTestId(TestId.ClientId));

            var updated = await Api.UpdateProjectAsync(_todelete.Id, _todelete.ClientId, name: "Test Updated Project", notes: "notes");

            // stuff changed
            Assert.NotEqual(_todelete.Name, updated.Name);
            Assert.Equal("Test Updated Project", updated.Name);
            Assert.NotEqual(_todelete.Notes, updated.Notes);
            Assert.Equal("notes", updated.Notes);

            // stuff didn't change
            Assert.Equal(_todelete.Active, updated.Active);
            Assert.Equal(_todelete.BillBy, updated.BillBy);
            Assert.Equal(_todelete.Budget, updated.Budget);
            Assert.Equal(_todelete.ClientId, updated.ClientId);
        }
        #endregion
        public void Dispose()
        {
            if (_todelete != null)
            {
                Api.DeleteProject(_todelete.Id);
            }
        }
    }
}
