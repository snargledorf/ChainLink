using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ChainLink.ChainBuilders
{
    internal class InputChainBuilder<T> : InputChainBuilderBase<T>, IRootInputChainBuilder<T>
    {
        public IRunChainLinkRunner<T>[] Build() => Children.Select(c => c.CreateChainLinkRunner()).Cast<IRunChainLinkRunner<T>>().ToArray();

        public IInputRunChainBuilder<T, T, TChainLink> RunWithInput<TChainLink>(params object[] args) where TChainLink : IRunChainLink<T>
        {
            return AddChildChainBuilder(new InputRunChainBuilder<T, T, TChainLink>(ReflectionUtils.CreateObject<TChainLink>(args), this));
        }

        public IInputRunChainBuilder<T, T, TChainLink> RunWithInput<TChainLink>(TChainLink chainLink) where TChainLink : IRunChainLink<T>
        {
            return AddChildChainBuilder(new InputRunChainBuilder<T, T, TChainLink>(chainLink, this));
        }

        public IInputRunResultChainBuilder<T, T, TResult, TChainLink> RunWithInput<TResult, TChainLink>(params object[] args) where TChainLink : IRunChainLink<T>, IResultChainLink<TResult>
        {
            return AddChildChainBuilder(new InputRunResultChainBuilder<T, T, TResult, TChainLink>(ReflectionUtils.CreateObject<TChainLink>(args), this));
        }

        public IInputRunResultChainBuilder<T, T, TResult, TChainLink> RunWithInput<TResult, TChainLink>(TChainLink chainLink) where TChainLink : IRunChainLink<T>, IResultChainLink<TResult>
        {
            return AddChildChainBuilder(new InputRunResultChainBuilder<T, T, TResult, TChainLink>(chainLink, this));
        }

        public IInputRunChainBuilder<T, T, DelegateRunChainLink<T>> RunWithInput(Action<T> del)
        {
            return RunWithInput((input, _, cancel) => Task.Run(() => del(input), cancel));
        }

        public IInputRunChainBuilder<T, T, DelegateRunChainLink<T>> RunWithInput(Func<T, Task> del)
        {
            return RunWithInput((input, _, __) => del(input));
        }

        public IInputRunChainBuilder<T, T, DelegateRunChainLink<T>> RunWithInput(Func<T, IChainLinkRunContext, Task> del)
        {
            return RunWithInput((input, context, _) => del(input, context));
        }

        public IInputRunChainBuilder<T, T, DelegateRunChainLink<T>> RunWithInput(Func<T, IChainLinkRunContext, CancellationToken, Task> del)
        {
            return AddChildChainBuilder(new InputDelegateRunChainBuilder<T, T>(del, this));
        }

        public IInputRunResultChainBuilder<T, T, TResult, DelegateRunResultChainLink<T, TResult>> RunWithInput<TResult>(Func<T, TResult> del)
        {
            return RunWithInput((input, _, cancel) => Task.Run(() => del(input), cancel));
        }

        public IInputRunResultChainBuilder<T, T, TResult, DelegateRunResultChainLink<T, TResult>> RunWithInput<TResult>(Func<T, Task<TResult>> del)
        {
            return RunWithInput((input, _, __) => del(input));
        }

        public IInputRunResultChainBuilder<T, T, TResult, DelegateRunResultChainLink<T, TResult>> RunWithInput<TResult>(Func<T, IChainLinkRunContext, TResult> del)
        {
            return RunWithInput((input, context, cancel) => Task.Run(() => del(input, context), cancel));
        }

        public IInputRunResultChainBuilder<T, T, TResult, DelegateRunResultChainLink<T, TResult>> RunWithInput<TResult>(Func<T, IChainLinkRunContext, Task<TResult>> del)
        {
            return RunWithInput((input, context, _) => del(input, context));
        }

        public IInputRunResultChainBuilder<T, T, TResult, DelegateRunResultChainLink<T, TResult>> RunWithInput<TResult>(Func<T, IChainLinkRunContext, CancellationToken, TResult> del)
        {
            return RunWithInput((input, context, cancel) => Task.Run(() => del(input, context, cancel), cancel));
        }

        public IInputRunResultChainBuilder<T, T, TResult, DelegateRunResultChainLink<T, TResult>> RunWithInput<TResult>(Func<T, IChainLinkRunContext, CancellationToken, Task<TResult>> del)
        {
            return AddChildChainBuilder(new InputDelegateRunResultChainBuilder<T, T, TResult, DelegateRunResultChainLink<T, TResult>>(del, this));
        }

        public IInputRunResultChainBuilder<T, T, T, IfChainLink<T>> If(Func<T, bool> condition)
        {
            return If((input, _, cancel) => Task.Run(() => condition(input), cancel));
        }

        public IInputRunResultChainBuilder<T, T, T, IfChainLink<T>> If(Func<T, Task<bool>> condition)
        {
            return If((input, _, __) => condition(input));
        }

        public IInputRunResultChainBuilder<T, T, T, IfChainLink<T>> If(Func<T, IChainLinkRunContext, bool> condition)
        {
            return If((input, context, cancel) => Task.Run(() => condition(input, context), cancel));
        }

        public IInputRunResultChainBuilder<T, T, T, IfChainLink<T>> If(Func<T, IChainLinkRunContext, Task<bool>> condition)
        {
            return If((input, context, _) => condition(input, context));
        }

        public IInputRunResultChainBuilder<T, T, T, IfChainLink<T>> If(Func<T, IChainLinkRunContext, CancellationToken, bool> condition)
        {
            return If((input, context, cancel) => Task.Run(() => condition(input, context, cancel), cancel));
        }

        public IInputRunResultChainBuilder<T, T, T, IfChainLink<T>> If(Func<T, IChainLinkRunContext, CancellationToken, Task<bool>> condition)
        {
            return AddChildChainBuilder(new InputIfChainBuilder<T, T>(condition));
        }

        public IInputRunResultChainBuilder<T, T, T, IfChainLink<T>> If(Func<bool> condition)
        {
            return If((_, cancel) => Task.Run(() => condition(), cancel));
        }

        public IInputRunResultChainBuilder<T, T, T, IfChainLink<T>> If(Func<Task<bool>> condition)
        {
            return If((_, __, ___) => condition());
        }

        public IInputRunResultChainBuilder<T, T, T, IfChainLink<T>> If(Func<IChainLinkRunContext, bool> condition)
        {
            return If((_, context, cancel) => Task.Run(() => condition(context), cancel));
        }

        public IInputRunResultChainBuilder<T, T, T, IfChainLink<T>> If(Func<IChainLinkRunContext, Task<bool>> condition)
        {
            return If((_, context, __) => condition(context));
        }

        public IInputRunResultChainBuilder<T, T, T, IfChainLink<T>> If(Func<IChainLinkRunContext, CancellationToken, bool> condition)
        {
            return If((_, context, cancel) => Task.Run(() => condition(context, cancel), cancel));
        }

        public IInputRunResultChainBuilder<T, T, T, IfChainLink<T>> If(Func<IChainLinkRunContext, CancellationToken, Task<bool>> condition)
        {
            return If((_, context, cancel) => condition(context, cancel));
        }
    }
}
