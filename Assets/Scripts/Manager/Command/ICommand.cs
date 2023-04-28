namespace Manager.Command
{
    public interface ICommand
    {
        /// <summary>
        /// 実行
        /// </summary>
        public void Execute();

        /// <summary>
        /// 実行巻き戻し
        /// </summary>
        public void Undo();
    }
}