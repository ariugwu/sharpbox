namespace sharpbox.WebLibrary.Core
{
    public abstract class LifecycleHandler<T>
    {
        protected LifecycleHandler<T> _successor;

        /// <summary>
        /// The set successor.
        /// </summary>
        /// <param name="successor">
        /// The successor.
        /// </param>
        public void SetSuccessor(LifecycleHandler<T> successor)
        {
            this._successor = successor;
        }

        public abstract void HandleRequest(WebContext<T> webContext);
    }
}
