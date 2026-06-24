namespace Project.Middleware
{
    using System;

    // / <summary>
    // / Base class for implementing <see cref="IDisposable"/>.
    // / </summary>
    // / <remarks>
    // / Provides a base class that can be used as a common means for implementing <see cref="IDisposable"/>.
    // / </remarks>
    public abstract class DisposableBase
    {
        // / <summary>
        // / Gets or sets a value indicating whether this instance is disposed.
        // / </summary>
        // / <value>
        // /   <c>true</c> if this instance is disposed; otherwise, <c>false</c>.
        // / </value>
        protected bool IsDisposed { get; set; }

        // / <summary>
        // / Releases unmanaged and - optionally - managed resources.
        // / </summary>
        public virtual void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        // / <summary>
        // / Releases unmanaged and - optionally - managed resources.
        // / </summary>
        // / <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected abstract void Dispose(bool disposing);

        // / <summary>
        // / Invokes the dispose action.
        // / </summary>
        // / <param name="disposing">if set to <c>true</c> [disposing].</param>
        // / <param name="disposeAction">The dispose action.</param>
        // / <remarks>
        // / This method can be used to pass a lambda action that is then invoked upon disposing.
        // / This means that each implementing class can provide it's own action for what to do when it is being disposed.
        // / </remarks>
        // / <example>
        // / Here is an example that passes some clean-up logic via a the disposeAction parameter.
        // / <code>
        // / protected override Dispose(bool disposing)
        // / {
        // /     this.InvokeDisposeAction(
        // /         disposing,
        // /         () =>
        // /         {
        // /             this.unitOfWork.DisposeIfNotNull();
        // /         });
        // / }
        // / </code>
        // / </example>
        protected virtual void InvokeDisposeAction(bool disposing, Action disposeAction = null)
        {
            if (!this.IsDisposed)
            {
                if (disposing)
                {
                    disposeAction?.Invoke();
                }
            }

            this.IsDisposed = true;
        }
    }
}
