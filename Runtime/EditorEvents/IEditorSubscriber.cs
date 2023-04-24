namespace MUtility
{
	public interface IEditorSubscriber
	{
		void Subscribe();
		bool IsSubscribedAlready { get; }
	}
}