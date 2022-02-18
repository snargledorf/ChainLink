using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ChainLink.ChainBuilders
{
    internal class DelegateRunChainBuilder : ChainBuilderBase, IChainLinkRunnerFactory, IRunChainBuilder<DelegateRunChainLink>
    {
        private readonly Func<IChainLinkRunContext, CancellationToken, Task> del;

        public DelegateRunChainBuilder(Func<IChainLinkRunContext, CancellationToken, Task> del, ChainBuilderBase previous = null)
            : base(previous)
        {
            this.del = del;
        }

        public IChainLinkRunner CreateChainLinkRunner()
        {
            return new RunChainLinkRunner(new DelegateRunChainLink(del), Children.Select(c => c.CreateChainLinkRunner()).ToArray());
        }
    }
    internal class DelegateRunChainBuilder<T> : ChainBuilderBase, IChainLinkRunnerFactory, IRunChainBuilder<T, DelegateRunChainLink<T>>
    {
        private readonly Func<T, IChainLinkRunContext, CancellationToken, Task> del;

        public DelegateRunChainBuilder(Func<T, IChainLinkRunContext, CancellationToken, Task> del, ChainBuilderBase previous = null)
            : base(previous)
        {
            this.del = del;
        }

        public IChainLinkRunner CreateChainLinkRunner()
        {
            return new RunChainLinkRunner<T>(new DelegateRunChainLink<T>(del), Children.Select(c => c.CreateChainLinkRunner()).ToArray());
        }
    }

    internal class DelegateRunResultChainBuilder<T> : ChainBuilderBase, IChainLinkRunnerFactory, IRunResultChainBuilder<T, DelegateRunResultChainLink<T>>
    {
        private readonly Func<IChainLinkRunContext, CancellationToken, Task<T>> del;

        public DelegateRunResultChainBuilder(Func<IChainLinkRunContext, CancellationToken, Task<T>> del, ChainBuilderBase previous = null)
            : base(previous)
        {
            this.del = del;
        }

        public IChainLinkRunner CreateChainLinkRunner()
        {
            return new RunResultChainLinkRunner<T, DelegateRunResultChainLink<T>>(
                new DelegateRunResultChainLink<T>(del),
                Children.Select(c => c.CreateChainLinkRunner()).ToArray());
        }

        public IRunChainBuilder<T, TNewChainLink> RunWithResult<TNewChainLink>(params object[] args) where TNewChainLink : IRunChainLink<T>
        {
            return AddChildChainBuilder(new RunChainBuilder<T, TNewChainLink>(args, this));
        }

        public IRunResultChainBuilder<T, TResult, TNewChainLink> RunWithResult<TResult, TNewChainLink>(params object[] args) where TNewChainLink : IRunChainLink<T>, IResultChainLink<TResult>
        {
            return AddChildChainBuilder(new RunResultChainBuilder<T, TResult, TNewChainLink>(args, this));
        }

        public IRunChainBuilder<T, DelegateRunChainLink<T>> RunWithResult(Action<T> del)
        {
            return RunWithResult((input, _, cancel) => Task.Run(() => del(input), cancel));
        }

        public IRunChainBuilder<T, DelegateRunChainLink<T>> RunWithResult(Func<T, Task> del)
        {
            return RunWithResult((input, _, __) => del(input));
        }

        public IRunChainBuilder<T, DelegateRunChainLink<T>> RunWithResult(Func<T, IChainLinkRunContext, Task> del)
        {
            return RunWithResult((input, context, _) => del(input, context));
        }

        public IRunChainBuilder<T, DelegateRunChainLink<T>> RunWithResult(Func<T, IChainLinkRunContext, CancellationToken, Task> del)
        {
            return AddChildChainBuilder(new DelegateRunChainBuilder<T>(del, this));
        }

        public IRunResultChainBuilder<T, TResult, DelegateRunResultChainLink<T, TResult>> RunWithResult<TResult>(Func<T, TResult> del)
        {
            return RunWithResult((input, _, cancel) => Task.Run(() => del(input), cancel));
        }

        public IRunResultChainBuilder<T, TResult, DelegateRunResultChainLink<T, TResult>> RunWithResult<TResult>(Func<T, Task<TResult>> del)
        {
            return RunWithResult((input, _, __) => del(input));
        }

        public IRunResultChainBuilder<T, TResult, DelegateRunResultChainLink<T, TResult>> RunWithResult<TResult>(Func<T, IChainLinkRunContext, TResult> del)
        {
            return RunWithResult((input, context, cancel) => Task.Run(() => del(input, context), cancel));
        }

        public IRunResultChainBuilder<T, TResult, DelegateRunResultChainLink<T, TResult>> RunWithResult<TResult>(Func<T, IChainLinkRunContext, Task<TResult>> del)
        {
            return RunWithResult((input, context, _) => del(input, context));
        }

        public IRunResultChainBuilder<T, TResult, DelegateRunResultChainLink<T, TResult>> RunWithResult<TResult>(Func<T, IChainLinkRunContext, CancellationToken, TResult> del)
        {
            return RunWithResult((input, context, cancel) => Task.Run(() => del(input, context, cancel), cancel));
        }

        public IRunResultChainBuilder<T, TResult, DelegateRunResultChainLink<T, TResult>> RunWithResult<TResult>(Func<T, IChainLinkRunContext, CancellationToken, Task<TResult>> del)
        {
            return AddChildChainBuilder(new DelegateRunResultChainBuilder<T, TResult, DelegateRunResultChainLink<T, TResult>>(del, this));
        }
    }

    internal class DelegateRunResultChainBuilder<TInput, TResult, TChainLink> : ChainBuilderBase, IChainLinkRunnerFactory, IRunResultChainBuilder<TInput, TResult, DelegateRunResultChainLink<TInput, TResult>>
        where TChainLink : IRunChainLink<TInput>, IResultChainLink<TResult>
    {
        private readonly Func<TInput, IChainLinkRunContext, CancellationToken, Task<TResult>> del;

        public DelegateRunResultChainBuilder(Func<TInput, IChainLinkRunContext, CancellationToken, Task<TResult>> del, ChainBuilderBase previous = null)
            : base(previous)
        {
            this.del = del;
        }

        public IChainLinkRunner CreateChainLinkRunner()
        {
            return new RunResultChainLinkRunner<TInput, TResult, DelegateRunResultChainLink<TInput, TResult>>(
                new DelegateRunResultChainLink<TInput, TResult>(del),
                Children.Select(c => c.CreateChainLinkRunner()).ToArray());
        }

        public IRunChainBuilder<TResult, TNewChainLink> RunWithResult<TNewChainLink>(params object[] args) 
            where TNewChainLink : IRunChainLink<TResult>
        {
            return AddChildChainBuilder(new RunChainBuilder<TResult, TNewChainLink>(args, this));
        }

        public IRunResultChainBuilder<TResult, T, TNewChainLink> RunWithResult<T, TNewChainLink>(params object[] args)
            where TNewChainLink : IRunChainLink<TResult>, IResultChainLink<T>
        {
            return AddChildChainBuilder(new RunResultChainBuilder<TResult, T, TNewChainLink>(args, this));
        }

        public IRunChainBuilder<TResult, DelegateRunChainLink<TResult>> RunWithResult(Action<TResult> del)
        {
            return RunWithResult((input, _, cancel) => Task.Run(() => del(input), cancel));
        }

        public IRunChainBuilder<TResult, DelegateRunChainLink<TResult>> RunWithResult(Func<TResult, Task> del)
        {
            return RunWithResult((input, _, __) => del(input));
        }

        public IRunChainBuilder<TResult, DelegateRunChainLink<TResult>> RunWithResult(Func<TResult, IChainLinkRunContext, Task> del)
        {
            return RunWithResult((input, context, _) => del(input, context));
        }

        public IRunChainBuilder<TResult, DelegateRunChainLink<TResult>> RunWithResult(Func<TResult, IChainLinkRunContext, CancellationToken, Task> del)
        {
            return AddChildChainBuilder(new DelegateRunChainBuilder<TResult>(del, this));
        }

        public IRunResultChainBuilder<TResult, T, DelegateRunResultChainLink<TResult, T>> RunWithResult<T>(Func<TResult, T> del)
        {
            return RunWithResult((input, _, cancel) => Task.Run(() => del(input), cancel));
        }

        public IRunResultChainBuilder<TResult, T, DelegateRunResultChainLink<TResult, T>> RunWithResult<T>(Func<TResult, Task<T>> del)
        {
            return RunWithResult((input, _, __) => del(input));
        }

        public IRunResultChainBuilder<TResult, T, DelegateRunResultChainLink<TResult, T>> RunWithResult<T>(Func<TResult, IChainLinkRunContext, T> del)
        {
            return RunWithResult((input, context, cancel) => Task.Run(() => del(input, context), cancel));
        }

        public IRunResultChainBuilder<TResult, T, DelegateRunResultChainLink<TResult, T>> RunWithResult<T>(Func<TResult, IChainLinkRunContext, Task<T>> del)
        {
            return RunWithResult((input, context, _) => del(input, context));
        }

        public IRunResultChainBuilder<TResult, T, DelegateRunResultChainLink<TResult, T>> RunWithResult<T>(Func<TResult, IChainLinkRunContext, CancellationToken, T> del)
        {
            return RunWithResult((input, context, cancel) => Task.Run(() => del(input, context, cancel), cancel));
        }

        public IRunResultChainBuilder<TResult, T, DelegateRunResultChainLink<TResult, T>> RunWithResult<T>(Func<TResult, IChainLinkRunContext, CancellationToken, Task<T>> del)
        {
            return AddChildChainBuilder(new DelegateRunResultChainBuilder<TResult, T, DelegateRunResultChainLink<TResult, T>>(del, this));
        }
    }

}
