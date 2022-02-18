using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ChainLink.ChainBuilders
{
    internal class InputChainBuilder<T> : InputChainBuilderBase<T>, IRootInputChainBuilder<T>
    {
        public IRunChainLinkRunner<T>[] Build() => Children.Select(c => c.CreateChainLinkRunner()).Cast<IRunChainLinkRunner<T>>().ToArray();

        // TODO Figure out a way to not have to use explicit implementation
        IInputRunChainBuilder<T, T, TChainLink> IRootInputChainBuilder<T>.Run<TChainLink>(params object[] args)
        {
            return AddChildChainBuilder(new InputRunChainBuilder<T, T, TChainLink>(args, this));
        }

        // TODO Figure out a way to not have to use explicit implementation
        IInputRunResultChainBuilder<T, T, TResult, TChainLink> IRootInputChainBuilder<T>.Run<TResult, TChainLink>(params object[] args)
        {
            return AddChildChainBuilder(new InputRunResultChainBuilder<T, T, TResult, TChainLink>(args, this));
        }

        public IInputRunChainBuilder<T, T, DelegateRunChainLink<T>> Run(Action<T> del)
        {
            return Run((input, _, cancel) => Task.Run(() => del(input), cancel));
        }

        public IInputRunChainBuilder<T, T, DelegateRunChainLink<T>> Run(Func<T, Task> del)
        {
            return Run((input, _, __) => del(input));
        }

        public IInputRunChainBuilder<T, T, DelegateRunChainLink<T>> Run(Func<T, IChainLinkRunContext, Task> del)
        {
            return Run((input, context, _) => del(input, context));
        }

        public IInputRunChainBuilder<T, T, DelegateRunChainLink<T>> Run(Func<T, IChainLinkRunContext, CancellationToken, Task> del)
        {
            return AddChildChainBuilder(new InputDelegateRunChainBuilder<T, T>(del, this));
        }

        public IInputRunResultChainBuilder<T, T, TResult, DelegateRunResultChainLink<T, TResult>> Run<TResult>(Func<T, TResult> del)
        {
            return Run((input, _, cancel) => Task.Run(() => del(input), cancel));
        }

        public IInputRunResultChainBuilder<T, T, TResult, DelegateRunResultChainLink<T, TResult>> Run<TResult>(Func<T, Task<TResult>> del)
        {
            return Run((input, _, __) => del(input));
        }

        public IInputRunResultChainBuilder<T, T, TResult, DelegateRunResultChainLink<T, TResult>> Run<TResult>(Func<T, IChainLinkRunContext, TResult> del)
        {
            return Run((input, context, cancel) => Task.Run(() => del(input, context), cancel));
        }

        public IInputRunResultChainBuilder<T, T, TResult, DelegateRunResultChainLink<T, TResult>> Run<TResult>(Func<T, IChainLinkRunContext, Task<TResult>> del)
        {
            return Run((input, context, _) => del(input, context));
        }

        public IInputRunResultChainBuilder<T, T, TResult, DelegateRunResultChainLink<T, TResult>> Run<TResult>(Func<T, IChainLinkRunContext, CancellationToken, TResult> del)
        {
            return Run((input, context, cancel) => Task.Run(() => del(input, context, cancel), cancel));
        }

        public IInputRunResultChainBuilder<T, T, TResult, DelegateRunResultChainLink<T, TResult>> Run<TResult>(Func<T, IChainLinkRunContext, CancellationToken, Task<TResult>> del)
        {
            return AddChildChainBuilder(new InputDelegateRunResultChainBuilder<T, T, TResult, DelegateRunResultChainLink<T, TResult>>(del, this));
        }
    }
}
