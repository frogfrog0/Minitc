using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace projekt_2_na_punkty.Presenter
{
    internal class presenter
    {
        private Form1 _view;
        private Model.Totalcommander _model;
        public presenter(Form1 view, Model.Totalcommander model)
        {
            _view = view;
            _model = model;
            _view.AvailableDrives = _model.Dyski();
            _view.Setdisk += _view_Setdisk;
            _view.Setpath += _view_Setpath;
            _view.Setdisk2 += _view_Setdisk2;
            _view.Setpath2 += _view_Setpath2;
            _view.Copybutton += copyfile;
        }
        private void _view_Setdisk(string fragment) {
            _model.acdysk(fragment);
            updatetree();
            updatepath();
        }
        private void _view_Setpath(string frag)
        {
            if (_model.setpath(frag) == "d")
            {
                updatepath();
                updatetree();
            }
        } 
        private void updatepath()
        {
            _view.LeftPanelPath = _model.acpath();
        }
        private void updatetree() {

            _view.LeftPanelContent = _model.treecontent();
        }
        private void _view_Setdisk2(string fragment)
        {
            _model.acdysk2(fragment);
            updatetree2();
            updatepath2();
        }
        private void _view_Setpath2(string frag)
        {
            if (_model.setpath2(frag) == "d")
            {
                updatepath2();
                updatetree2();
            }
        }
        private void updatepath2()
        {
            _view.RightPanelPath = _model.acpath2();
        }
        private void updatetree2()
        {

            _view.RightPanelContent = _model.treecontent2();
        }
        private void copyfile()
        {
            _model.Copy();
        }
    }
}
