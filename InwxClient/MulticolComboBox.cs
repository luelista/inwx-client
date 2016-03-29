using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace DbScriptApp {
    public class MulticolComboItem {
        public string[] Columns { get; set; }
        public override string ToString() {
            return Columns[0];
        }
        public MulticolComboItem(string[] columns) {
            if (columns == null || columns.Length == 0) throw new ArgumentException("columns must have length>=1");
            this.Columns = columns;
        }
    }
    public class MulticolComboBox : ComboBox {
        private int[] _columnWidths;
        public int[] ColumnWidths {
            get {
                return _columnWidths;
            }
            set {
                DropDownWidth = value.Sum();
                _columnWidths = value;
            }
        }

        public MulticolComboBox()
            : base() {
            ColumnWidths = new int[] { 150 };
            this.DrawMode = DrawMode.OwnerDrawFixed;
        }
        protected override void OnDrawItem(DrawItemEventArgs e) {
            if (e.Index == -1)
                return;

            using (SolidBrush brush = new SolidBrush(e.ForeColor)) {
                Font font = e.Font;
                //if (/*Condition Specifying That Text Must Be Bold*/)
                //    font = new System.Drawing.Font(font, FontStyle.Bold);
                e.DrawBackground();
                string[] cols;
                object item = Items[e.Index];
                if (item is MulticolComboItem) cols = ((MulticolComboItem)item).Columns;
                else cols = new string[] { Convert.ToString(item) };
                Rectangle b = e.Bounds;
                for (int i = 0; i < cols.Length && i < ColumnWidths.Length; i++) {
                    b.Width = ColumnWidths[i];
                    e.Graphics.DrawString(cols[i], font, brush, b);
                    b.X += ColumnWidths[i];
                }

                e.DrawFocusRectangle();
            }
        }
    }
}
