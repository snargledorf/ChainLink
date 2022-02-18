﻿using System;
using System.Threading;
using System.Threading.Tasks;

namespace ChainLink.ChainBuilders
{
    internal abstract class ResultChainBuilderBase<T, TChainLink> : ChainBuilderBase<TChainLink>, IResultChainBuilder<T, TChainLink>
        where TChainLink : IResultChainLink<T>
    {
        protected ResultChainBuilderBase(object[] args, ChainBuilderBase previous = null)
            : base(args, previous)
        {
        }

        new public IRunResultChainBuilder<T, T, RunChainLinkPassInputWrapper<T, TNewChainLink>> Run<TNewChainLink>(params object[] args)
            where TNewChainLink : IRunChainLink
        {
            var wrapper = new RunChainLinkWrapperArgs(args);
            return AddChildChainBuilder(new RunResultChainBuilder<T, T, RunChainLinkPassInputWrapper<T, TNewChainLink>>(new[] { wrapper }, this));
        }

        public IRunChainBuilder<T, TNewChainLink> RunWithInput<TNewChainLink>(params object[] args)
            where TNewChainLink : IRunChainLink<T>
        {
            return AddChildChainBuilder(new RunChainBuilder<T, TNewChainLink>(args, this));
        }

        public IRunResultChainBuilder<T, TResult, TNewChainLink> RunWithInput<TResult, TNewChainLink>(params object[] args)
            where TNewChainLink : IRunChainLink<T>, IResultChainLink<TResult>
        {
            return AddChildChainBuilder(new RunResultChainBuilder<T, TResult, TNewChainLink>(args, this));
        }

        new public IRunResultChainBuilder<T, T, DelegateRunChainLink<T>> Run(Action del)
        {
            return Run((_, cancel) => Task.Run(del, cancel));
        }

        new public IRunResultChainBuilder<T, T, DelegateRunChainLink<T>> Run(Func<Task> del)
        {
            return Run((_, __) => del());
        }

        new public IRunResultChainBuilder<T, T, DelegateRunChainLink<T>> Run(Func<IChainLinkRunContext, Task> del)
        {
            return Run((context, _) => del(context));
        }

        new public IRunResultChainBuilder<T, T, DelegateRunChainLink<T>> Run(Func<IChainLinkRunContext, CancellationToken, Task> del)
        {
            return AddChildChainBuilder(new DelegateRunChainBuilder<T>(del, this));
        }

        public IRunResultChainBuilder<T, T, DelegateWithInputRunChainLink<T>> RunWithInput(Action<T> del)
        {
            return RunWithInput((input, _, cancel) => Task.Run(() => del(input), cancel));
        }

        public IRunResultChainBuilder<T, T, DelegateWithInputRunChainLink<T>> RunWithInput(Func<T, Task> del)
        {
            return RunWithInput((input, _, __) => del(input));
        }

        public IRunResultChainBuilder<T, T, DelegateWithInputRunChainLink<T>> RunWithInput(Func<T, IChainLinkRunContext, Task> del)
        {
            return RunWithInput((input, context, _) => del(input, context));
        }

        public IRunResultChainBuilder<T, T, DelegateWithInputRunChainLink<T>> RunWithInput(Func<T, IChainLinkRunContext, CancellationToken, Task> del)
        {
            return AddChildChainBuilder(new DelegateRunWithInputChainBuilder<T>(del, this));
        }

        public IRunResultChainBuilder<T, TResult, DelegateWithInputRunResultChainLink<T, TResult>> RunWithInput<TResult>(Func<T, TResult> del)
        {
            return RunWithInput((input, _, cancel) => Task.Run(() => del(input), cancel));
        }

        public IRunResultChainBuilder<T, TResult, DelegateWithInputRunResultChainLink<T, TResult>> RunWithInput<TResult>(Func<T, Task<TResult>> del)
        {
            return RunWithInput((input, _, __) => del(input));
        }

        public IRunResultChainBuilder<T, TResult, DelegateWithInputRunResultChainLink<T, TResult>> RunWithInput<TResult>(Func<T, IChainLinkRunContext, TResult> del)
        {
            return RunWithInput((input, context, cancel) => Task.Run(() => del(input, context), cancel));
        }

        public IRunResultChainBuilder<T, TResult, DelegateWithInputRunResultChainLink<T, TResult>> RunWithInput<TResult>(Func<T, IChainLinkRunContext, Task<TResult>> del)
        {
            return RunWithInput((input, context, _) => del(input, context));
        }

        public IRunResultChainBuilder<T, TResult, DelegateWithInputRunResultChainLink<T, TResult>> RunWithInput<TResult>(Func<T, IChainLinkRunContext, CancellationToken, TResult> del)
        {
            return RunWithInput((input, context, cancel) => Task.Run(() => del(input, context, cancel), cancel));
        }

        public IRunResultChainBuilder<T, TResult, DelegateWithInputRunResultChainLink<T, TResult>> RunWithInput<TResult>(Func<T, IChainLinkRunContext, CancellationToken, Task<TResult>> del)
        {
            return AddChildChainBuilder(new DelegateWithInputRunResultChainBuilder<T, TResult, DelegateWithInputRunResultChainLink<T, TResult>>(del, this));
        }

        new public IRunResultChainBuilder<T, T, IfChainLink<T>> If(Func<bool> condition)
        {
            return If((_, __, cancel) => Task.Run(() => condition(), cancel));
        }

        new public IRunResultChainBuilder<T, T, IfChainLink<T>> If(Func<Task<bool>> condition)
        {
            return If((_, __, ___) => condition());
        }

        new public IRunResultChainBuilder<T, T, IfChainLink<T>> If(Func<IChainLinkRunContext, bool> condition)
        {
            return If((_, context, cancel) => Task.Run(() => condition(context), cancel));
        }

        new public IRunResultChainBuilder<T, T, IfChainLink<T>> If(Func<IChainLinkRunContext, Task<bool>> condition)
        {
            return If((_, context, __) => condition(context));
        }

        new public IRunResultChainBuilder<T, T, IfChainLink<T>> If(Func<IChainLinkRunContext, CancellationToken, bool> condition)
        {
            return If((_, context, cancel) => Task.Run(() => condition(context, cancel), cancel));
        }

        new public IRunResultChainBuilder<T, T, IfChainLink<T>> If(Func<IChainLinkRunContext, CancellationToken, Task<bool>> condition)
        {
            return If((_, context, cancel) => condition(context, cancel));
        }

        public IRunResultChainBuilder<T, T, IfChainLink<T>> If(Func<T, bool> condition)
        {
            return If((input, _, cancel) => Task.Run(() => condition(input), cancel));
        }

        public IRunResultChainBuilder<T, T, IfChainLink<T>> If(Func<T, Task<bool>> condition)
        {
            return If((input, _, __) => condition(input));
        }

        public IRunResultChainBuilder<T, T, IfChainLink<T>> If(Func<T, IChainLinkRunContext, bool> condition)
        {
            return If((input, context, cancel) => Task.Run(() => condition(input, context), cancel));
        }

        public IRunResultChainBuilder<T, T, IfChainLink<T>> If(Func<T, IChainLinkRunContext, Task<bool>> condition)
        {
            return If((input, context, _) => condition(input, context));
        }

        public IRunResultChainBuilder<T, T, IfChainLink<T>> If(Func<T, IChainLinkRunContext, CancellationToken, bool> condition)
        {
            return If((input, context, cancel) => Task.Run(() => condition(input, context, cancel), cancel));
        }

        public IRunResultChainBuilder<T, T, IfChainLink<T>> If(Func<T, IChainLinkRunContext, CancellationToken, Task<bool>> condition)
        {
            return AddChildChainBuilder(new IfChainBuilder<T>(condition, this));
        }
    }
}
