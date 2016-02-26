using Microsoft.TeamFoundation.DistributedTask.WebApi;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.VisualStudio.Services.Agent.Worker
{
    public class JobRunner
    {
        public JobRunner(IHostContext hostContext) {
            m_hostContext = hostContext;
            m_trace = hostContext.GetTrace("JobRunner");
        }

        public async Task Run(JobRequestMessage message)
        {
            ExecutionContext context = new ExecutionContext(m_hostContext);
            m_trace.Verbose("Prepare");
            context.LogInfo("Prepare...");
            context.LogVerbose("Preparing...");
            
            m_trace.Verbose("Run");
            context.LogInfo("Run...");
            context.LogVerbose("Running...");
            
            m_trace.Verbose("Finish");
            context.LogInfo("Finish...");
            context.LogVerbose("Finishing...");

            m_trace.Info("Job id {0}", message.JobId);
            await Task.Yield();
            m_finishedSignal.Release();
        }

        public Task WaitToFinish(IHostContext context)
        {
            return m_finishedSignal.WaitAsync(context.CancellationToken);
        }

        private IHostContext m_hostContext;
        private readonly TraceSource m_trace;
        private SemaphoreSlim m_finishedSignal = new SemaphoreSlim(0, 1);
    }
}