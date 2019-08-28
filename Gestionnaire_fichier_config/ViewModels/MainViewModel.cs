using Gestionnaire_fichier_config.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Toolbox.MVVM.Commands;
using Toolbox.MVVM.ViewModels;
using Toolbox.Patterns.Mediator;
using GF = Toolbox.Fichiers.GestionFichiers;
using GC = Toolbox.Controls.GestionControls;
using GW = Toolbox.GestionWindow.GestionWindow;

namespace Gestionnaire_fichier_config.ViewModels
{
	public class MainViewModel : ViewModelBase
	{

        #region Propriétés
        //===============================================================================================================================//
        private string _theme_Path;
		public string Theme_Path
		{
			get { return _theme_Path; }
			set
			{
				if (value != _theme_Path)
				{
					_theme_Path = value;
					RaisePropertyChanged(nameof(Theme_Path));
				}
			}
		}
        //===============================================================================================================================//
        private string _config_Path;
		public string Config_Path
		{
			get { return _config_Path; }
			set
			{
				if (value != _config_Path)
				{
					_config_Path = value;
					RaisePropertyChanged(nameof(Config_Path));
				}
			}
		}
        //===============================================================================================================================//
        private Charged_Image_Window _cIW;
        public Charged_Image_Window CIW_Instance
        {
            get
            {
                return _cIW;
            }
            set
            {
                if(value != _cIW)
                _cIW = value; RaisePropertyChanged(nameof(CIW_Instance));

            }
        }
        //===============================================================================================================================//
        private Window _mW_Instance;
        public Window MW_Instance
        {
            get
            {
                return _mW_Instance;
            }
            set
            {
                if(value != _mW_Instance)
                {
                    _mW_Instance = value;
                    RaisePropertyChanged(nameof(MW_Instance));
                }
            }
        }
        #endregion

        #region Listes

        public ObservableCollection<string> Images_Paths { get; set; }
		public ObservableCollection<string> Config_Content { get; set; }

        #endregion

        #region Commandes
        //Get_Config_Path_Command===============================================================================================================================//
        private RelayCommand _get_Config_Path_Command;
		public RelayCommand Get_Config_Path_Command
		{
			get { return _get_Config_Path_Command ?? (_get_Config_Path_Command = new RelayCommand(Browse_Config_File, null)); }

		}

        //Get_Theme_Path_Command===============================================================================================================================//
        private RelayCommand _get_Theme_Path_Command;
		public RelayCommand Get_Theme_Path_Command
		{
			get { return _get_Theme_Path_Command ?? (_get_Theme_Path_Command = new RelayCommand(Browse_Theme_Folder, null)); }

		}

        //Load_Images_Command===============================================================================================================================//
        private RelayCommand _load_Images_Command;
		public RelayCommand Load_Images_Command
		{
			get { return _load_Images_Command ?? (_load_Images_Command = new RelayCommand(Load_Images, Can_Load_Images)); }
		}

        //Close_Window_Command===============================================================================================================================//
        private RelayCommand _close_Window_Command;
        public RelayCommand Close_Window_Command
        {
            get
            {
                return _close_Window_Command ?? (_close_Window_Command = new RelayCommand(Close_Window, null));
            }

        }
        #endregion

        public MainViewModel()
		{

			//On initialise directement comme ça le "Can_Load_Images" a juste à dire ' textbox != "" ' et
			//on s'ennuie pas avec le fait que les textboxs peuvent être (null ou vide) mais que les textboxs peuvent être "vide" ("")
			//Config_Path = @"C:\Users\maxim\Desktop\Dofus-Theme-Editor-exe\config.txt";
			Config_Path = @"";
            //Theme_Path = @"C:\Users\maxim\Desktop\Green";
            Theme_Path = @"";

            Images_Paths = new ObservableCollection<string>();
			Config_Content = new ObservableCollection<string>();

            MW_Instance = GW.Get_MainWindow_Instance();

            GC.Moving_Window(MW_Instance);

        }

        #region Fonctions
        //Browse_Config_File=========================================================================================//
        private void Browse_Config_File()
		{
			Config_Path = GF.Browse_File();
		}
        //Browse_Theme_Folder=========================================================================================//
        private void Browse_Theme_Folder()
		{
			Theme_Path = GF.Browse_Folder();
		}
        //Load_Images=========================================================================================//
        private void Load_Images()
		{

			Images_Paths = GF.Get_All_Files_Path(Theme_Path, Images_Paths, "png", "jpg", "jpeg");

			Load_Config();

			Display_CIW();

			Send_Lists_MVM_To_CIVM();

		}
        //Send_Lists_MVM_To_CIVM=========================================================================================//
        private void Send_Lists_MVM_To_CIVM()
		{
			Messenger<MainViewModel>.Instance.Send(Groups.ViewModels, this);
		}

        //Display_CIW=========================================================================================//

        private void Display_CIW()
		{
			CIW_Instance = new Charged_Image_Window();

			CIW_Instance.Show();
		}

        //Can_Load_Images=========================================================================================//
        private bool Can_Load_Images()
		{
			return (Theme_Path != "" && Config_Path != "");
		}
        //Load_Config=========================================================================================//
        private void Load_Config()
		{
			Config_Content = GF.Read_File_Content(Config_Path);
		}
        //Close_Window=========================================================================================//
        private void Close_Window()
        {
            GC.Close_Window(MW_Instance);
        }
        #endregion

    }
}
