
namespace TestPlatform.CrossPlatEngine.UnitTests.Execution
{
    using System;
    using System.Collections.ObjectModel;
    using System.Reflection;

    using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.EventHandlers;
    using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Execution;
    using Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine;
    using Microsoft.VisualStudio.TestPlatform.ObjectModel.InProcDataCollector;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;
    using Microsoft.VisualStudio.TestPlatform.ObjectModel;

    [TestClass]
    public class InProcDataCollectionExtensionManagerTests
    {
        private Mock<InProcDataCollectionExtensionManager> mockInProcDataCollectionHelper;

        private Mock<ITestCaseEventsHandler> mockTestCaseEvents;

        private TestCaseEventsHandler testCasesEventsHandler;

        [TestInitialize]
        public void InitializeTests()
        {
            this.mockInProcDataCollectionHelper = new Mock<InProcDataCollectionExtensionManager>(null);
            this.mockTestCaseEvents = new Mock<ITestCaseEventsHandler>();
            this.testCasesEventsHandler = new TestCaseEventsHandler(this.mockInProcDataCollectionHelper.Object, this.mockTestCaseEvents.Object);
        }

        [TestMethod]
        public void InProcDataCollectionTestCaseEventHandlerCallingTriggerTestCaseStart()
        {            
            this.testCasesEventsHandler.SendTestCaseStart(null);
            this.mockInProcDataCollectionHelper.Verify(x => x.TriggerTestCaseStart(null), Times.Once);
        }

        [TestMethod]
        public void InProcDataCollectionTestCaseEventHandlerCallingTriggerTestCaseEnd()
        {
            this.testCasesEventsHandler.SendTestCaseEnd(null, TestOutcome.Passed);
            this.mockInProcDataCollectionHelper.Verify(x => x.TriggerTestCaseEnd(null, TestOutcome.Passed), Times.Once);
        }

        [TestMethod]
        public void InProcDataCollectionTestCaseEventHandlerCallingTestCaseEventsFromClients()
        {
            this.testCasesEventsHandler.SendTestCaseStart(null);
            this.testCasesEventsHandler.SendTestCaseEnd(null, TestOutcome.Passed);
            this.testCasesEventsHandler.SendTestResult(null);

            this.mockTestCaseEvents.Verify(x => x.SendTestCaseStart(null), Times.Once);
            this.mockTestCaseEvents.Verify(x => x.SendTestCaseEnd(null, TestOutcome.Passed), Times.Once);
            this.mockTestCaseEvents.Verify(x => x.SendTestResult(null), Times.Once);
        }
    }
}