namespace Game.Model
{
    public partial class ModelBase
    {
        protected interface ICommand
        {
            void Exec(IContextWritable context);
        }
    }
}