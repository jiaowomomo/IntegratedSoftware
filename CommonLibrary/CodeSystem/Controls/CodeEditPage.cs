using CommonLibrary.CodeSystem.Common;
using ScintillaNET;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommonLibrary.CodeSystem.Controls
{
    public class CodeEditPage : Scintilla
    {
        private const int BACK_COLOR = 0x2A211C;
        private const int FORE_COLOR = 0xB7B7B7;
        private const int FOLDING_MARGIN = 3;
        private const bool CODEFOLDING_CIRCULAR = false;
        private const int BOOKMARK_MARGIN = 2;
        private const int BOOKMARK_MARKER = 2;

        private const string KEYWORD0 = "class extends implements import interface new case " +
                                        "do while else if for in switch throw get set function " +
                                        "var try catch finally while with default break continue " +
                                        "delete return each const namespace package include use is as " +
                                        "instanceof typeof author copy default deprecated eventType example " +
                                        "exampleText exception haxe inheritDoc internal link mtasc mxmlc param " +
                                        "private return see serial serialData serialField since throws usage version " +
                                        "langversion playerversion productversion dynamic private public partial static " +
                                        "intrinsic internal native override protected AS3 final super this arguments null " +
                                        "Infinity NaN undefined true false abstract as base bool break by byte case catch char " +
                                        "checked class const continue decimal default delegate do double descending explicit event " +
                                        "extern else enum false finally fixed float for foreach from goto group if implicit in int " +
                                        "interface internal into is lock long new null namespace object operator out override orderby " +
                                        "params private protected public readonly ref return switch struct sbyte sealed short sizeof " +
                                        "stackalloc static string select this throw true try typeof uint ulong unchecked unsafe ushort " +
                                        "using var virtual volatile void while where yield";

        private const string KEYWORD1 = "void Null ArgumentError arguments Array Boolean Class Date " +
                                        "DefinitionError Error EvalError Function int Math Namespace Number " +
                                        "Object RangeError ReferenceError RegExp SecurityError String SyntaxError " +
                                        "TypeError uint XML XMLList Boolean Byte Char DateTime Decimal Double Int16 " +
                                        "Int32 Int64 IntPtr SByte Single UInt16 UInt32 UInt64 UIntPtr Void Path File " +
                                        "System Windows Forms ScintillaNET";

        public bool CanEdit { get; set; } = false;
        public event Action CodeChanged = null;

        public CodeEditPage()
        {
            InitPage();
        }

        private void InitPage()
        {
            this.Dock = DockStyle.Fill;
            //初始视图配置
            this.WrapMode = WrapMode.None;
            this.IndentationGuides = IndentView.LookBoth;

            //配置样式
            InitColors();
            InitSyntaxColoring();

            //数字边距
            InitNumberMargin();

            //标记边距
            InitBookmarkMargin();

            //代码折叠边距
            InitCodeFolding();

            //初始化热键
            InitHotkeys();

            this.TextChanged += CodeEditPage_TextChanged;
        }

        private void InitColors()
        {
            this.SetSelectionBackColor(true, CodeEditorHelper.IntToColor(0x114D9C));//配置选中内容颜色
            this.CaretForeColor = Color.White;//设置光标颜色
        }

        private void InitSyntaxColoring()
        {
            //配置默认样式
            this.StyleResetDefault();
            this.Styles[Style.Default].Font = "Consolas";//字体格式
            this.Styles[Style.Default].Size = 15;//字体大小
            this.Styles[Style.Default].BackColor = Color.Black;//背景颜色
            this.Styles[Style.Default].ForeColor = CodeEditorHelper.IntToColor(0xFFFFFF);
            this.StyleClearAll();

            //配置CPP（C＃）词法分析器样式
            this.Styles[Style.Cpp.Identifier].ForeColor = CodeEditorHelper.IntToColor(0xD0DAE2);
            this.Styles[Style.Cpp.Comment].ForeColor = CodeEditorHelper.IntToColor(0xBD758B);
            this.Styles[Style.Cpp.CommentLine].ForeColor = CodeEditorHelper.IntToColor(0x40BF57);
            this.Styles[Style.Cpp.CommentDoc].ForeColor = CodeEditorHelper.IntToColor(0x2FAE35);
            this.Styles[Style.Cpp.Number].ForeColor = CodeEditorHelper.IntToColor(0xFFFF00);
            this.Styles[Style.Cpp.String].ForeColor = CodeEditorHelper.IntToColor(0xFFFF00);
            this.Styles[Style.Cpp.Character].ForeColor = CodeEditorHelper.IntToColor(0xE95454);
            this.Styles[Style.Cpp.Preprocessor].ForeColor = CodeEditorHelper.IntToColor(0x8AAFEE);
            this.Styles[Style.Cpp.Operator].ForeColor = CodeEditorHelper.IntToColor(0xE0E0E0);
            this.Styles[Style.Cpp.Regex].ForeColor = CodeEditorHelper.IntToColor(0xff00ff);
            this.Styles[Style.Cpp.CommentLineDoc].ForeColor = CodeEditorHelper.IntToColor(0x77A7DB);
            this.Styles[Style.Cpp.Word].ForeColor = CodeEditorHelper.IntToColor(0x48A8EE);
            this.Styles[Style.Cpp.Word2].ForeColor = CodeEditorHelper.IntToColor(0xF98906);
            this.Styles[Style.Cpp.CommentDocKeyword].ForeColor = CodeEditorHelper.IntToColor(0xB3D991);
            this.Styles[Style.Cpp.CommentDocKeywordError].ForeColor = CodeEditorHelper.IntToColor(0xFF0000);
            this.Styles[Style.Cpp.GlobalClass].ForeColor = CodeEditorHelper.IntToColor(0x48A8EE);

            this.Lexer = Lexer.Cpp;//设置词法分析器类型

            //设置关键字
            this.SetKeywords(0, KEYWORD0);
            this.SetKeywords(1, KEYWORD1);
        }

        private void InitNumberMargin()
        {
            this.Styles[Style.LineNumber].ForeColor = CodeEditorHelper.IntToColor(FORE_COLOR);
            this.Styles[Style.LineNumber].BackColor = CodeEditorHelper.IntToColor(BACK_COLOR);
            this.Styles[Style.IndentGuide].ForeColor = CodeEditorHelper.IntToColor(FORE_COLOR);
            this.Styles[Style.IndentGuide].BackColor = CodeEditorHelper.IntToColor(BACK_COLOR);

            var nums = this.Margins[1];
            nums.Width = 30;
            nums.Type = MarginType.Number;
            nums.Sensitive = false;
            nums.Mask = 0;
        }

        private void InitBookmarkMargin()
        {
            //this.SetFoldMarginColor(true, CodeEditorHelper.IntToColor(BACK_COLOR));

            var margin = this.Margins[BOOKMARK_MARGIN];
            margin.Width = 20;
            margin.Sensitive = true;
            margin.Type = MarginType.Symbol;
            margin.Mask = (1 << BOOKMARK_MARKER);
            //margin.Cursor = MarginCursor.Arrow;

            var marker = this.Markers[BOOKMARK_MARKER];
            marker.Symbol = MarkerSymbol.Circle;
            marker.SetBackColor(CodeEditorHelper.IntToColor(0xFF003B));
            marker.SetForeColor(CodeEditorHelper.IntToColor(0x000000));
            marker.SetAlpha(100);
        }

        private void InitCodeFolding()
        {
            this.SetFoldMarginColor(true, CodeEditorHelper.IntToColor(BACK_COLOR));
            this.SetFoldMarginHighlightColor(true, CodeEditorHelper.IntToColor(BACK_COLOR));

            //使能代码折叠
            this.SetProperty("fold", "1");
            this.SetProperty("fold.compact", "1");

            //配置边距显示折叠符号
            this.Margins[FOLDING_MARGIN].Type = MarginType.Symbol;
            this.Margins[FOLDING_MARGIN].Mask = Marker.MaskFolders;
            this.Margins[FOLDING_MARGIN].Sensitive = true;
            this.Margins[FOLDING_MARGIN].Width = 20;

            //设置所有折叠标记的颜色
            for (int i = 25; i <= 31; i++)
            {
                this.Markers[i].SetForeColor(CodeEditorHelper.IntToColor(BACK_COLOR)); // styles for [+] and [-]
                this.Markers[i].SetBackColor(CodeEditorHelper.IntToColor(FORE_COLOR)); // styles for [+] and [-]
            }

            //配置带有各自符号的折叠标记
            this.Markers[Marker.Folder].Symbol = CODEFOLDING_CIRCULAR ? MarkerSymbol.CirclePlus : MarkerSymbol.BoxPlus;
            this.Markers[Marker.FolderOpen].Symbol = CODEFOLDING_CIRCULAR ? MarkerSymbol.CircleMinus : MarkerSymbol.BoxMinus;
            this.Markers[Marker.FolderEnd].Symbol = CODEFOLDING_CIRCULAR ? MarkerSymbol.CirclePlusConnected : MarkerSymbol.BoxPlusConnected;
            this.Markers[Marker.FolderMidTail].Symbol = MarkerSymbol.TCorner;
            this.Markers[Marker.FolderOpenMid].Symbol = CODEFOLDING_CIRCULAR ? MarkerSymbol.CircleMinusConnected : MarkerSymbol.BoxMinusConnected;
            this.Markers[Marker.FolderSub].Symbol = MarkerSymbol.VLine;
            this.Markers[Marker.FolderTail].Symbol = MarkerSymbol.LCorner;

            //使能自动折叠
            this.AutomaticFold = (AutomaticFold.Show | AutomaticFold.Click | AutomaticFold.Change);
        }

        private void InitHotkeys()
        {
            // remove conflicting hotkeys from scintilla
            this.ClearCmdKey(Keys.Control | Keys.F);
            this.ClearCmdKey(Keys.Control | Keys.R);
            this.ClearCmdKey(Keys.Control | Keys.H);
            this.ClearCmdKey(Keys.Control | Keys.L);
            this.ClearCmdKey(Keys.Control | Keys.U);
        }

        private void CodeEditPage_TextChanged(object sender, EventArgs e)
        {
            if (CanEdit)
            {
                CodeChanged?.Invoke();
            }
        }
    }
}
