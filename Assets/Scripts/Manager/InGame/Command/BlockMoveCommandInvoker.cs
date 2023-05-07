using System.Collections.Generic;

namespace Manager.Command
{
    public class BlockMoveCommandInvoker
    {
        /// <summary>
        /// 行動履歴
        /// </summary>
        private static Stack<ICommand> _commands = new Stack<ICommand>();

        /// <summary>
        /// 実行
        /// </summary>
        /// <param name="command">実行してほしい命令</param>
        public static void Execute(ICommand command)
        {
            command.Execute();
            _commands.Push(command);
        }

        /// <summary>
        /// 実行巻き戻し
        /// </summary>
        public static void Undo()
        {
            //行動履歴があれば、コマンドを実行
            if (_commands.Count > 0)
            {
                ICommand command = _commands.Pop();
                command.Undo();
            }
        }
    }
}