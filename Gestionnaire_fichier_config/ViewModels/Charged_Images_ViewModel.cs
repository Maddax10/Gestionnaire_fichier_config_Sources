using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toolbox.MVVM.Commands;
using Toolbox.MVVM.ViewModels;
using Toolbox.Patterns.Mediator;
using GF = Toolbox.Fichiers.GestionFichiers;
using GI = Toolbox.Manipulation_Images.GestionImages;
using GC = Toolbox.Controls.GestionControls;
using Gestionnaire_fichier_config.Models;
using System.Windows.Media;
using System.Drawing;
using Gestionnaire_fichier_config.Views;
using System.Windows;

namespace Gestionnaire_fichier_config.ViewModels
{
    public class Charged_Images_ViewModel : ViewModelBase
    {

        #region Propriétés
        //=======================================================================================================================================================================//
        //Images_Paths===========================================================================================================================================================//
        //=======================================================================================================================================================================//
        private ObservableCollection<string> _images_Paths;

        public ObservableCollection<string> Images_Paths
        {
            get
            {
                return _images_Paths;
            }
            set
            {
                if(value != _images_Paths)
                {
                    _images_Paths = value;
                    RaisePropertyChanged(nameof(Images_Paths));
                }
            }
        }
        //=======================================================================================================================================================================//
        //Config_Content=========================================================================================================================================================//
        //=======================================================================================================================================================================//
        private ObservableCollection<string> _config_Content;

        public ObservableCollection<string> Config_Content
        {
            get
            {
                return _config_Content;
            }
            set
            {
                if(value != _config_Content)
                {
                    _config_Content = value;
                    RaisePropertyChanged(nameof(Config_Content));
                }
            }
        }
        //=======================================================================================================================================================================//
        //Theme_Path=============================================================================================================================================================//
        //=======================================================================================================================================================================//
        private string _theme_Path;

        public string Theme_Path
        {
            get
            {
                return _theme_Path;
            }
            set
            {

                if(value != _theme_Path)
                    _theme_Path = value;

                //Un problème survenait dans "Modify_Listbox_Stretch_Value_Config()" et "Modify_Listbox_Stretch_Value_FolderTheme()" entre le Theme_Path (qui valait par ex "C:\temp\images\lolilol" et le "overview_Config_Path"/"overview_FolderTheme_Path" qui valait "C:\temp\images\lolilol\" (un "\" de + à la fin)
                //Je vérifie que la fin du chemin d'accès n'ai pas déjà un "\"
                if(_theme_Path.Substring(_theme_Path.Length) != "\\")
                {
                    _theme_Path += "\\";
                }

                RaisePropertyChanged(nameof(Theme_Path));
            }
        }
        //=======================================================================================================================================================================//
        //SelectedItemConfig=====================================================================================================================================================//
        //=======================================================================================================================================================================//
        private ImageModel selectedItemConfig;

        public ImageModel SelectedItemConfig
        {
            get
            {
                return selectedItemConfig;
            }
            set
            {
                if(value != selectedItemConfig)
                {

                    selectedItemConfig = value;

                    RaisePropertyChanged(nameof(SelectedItemConfig));

                }

            }

        }
        //=======================================================================================================================================================================//
        //SelectedItemFolderTheme================================================================================================================================================//
        //=======================================================================================================================================================================//
        private ImageModel _selectedItemFolderTheme;

        public ImageModel SelectedItemFolderTheme
        {
            get
            {
                return _selectedItemFolderTheme;
            }
            set
            {
                if(value != _selectedItemFolderTheme)
                {

                    _selectedItemFolderTheme = value;

                    RaisePropertyChanged(nameof(SelectedItemFolderTheme));

                }

            }

        }

        //=======================================================================================================================================================================//
        //MVM_Instance===========================================================================================================================================================//
        //=======================================================================================================================================================================//
        private MainViewModel _mVM_Instance;

        public MainViewModel MVM_Instance
        {
            get
            {
                return _mVM_Instance;
            }
            set
            {
                if(value != _mVM_Instance)
                {

                    _mVM_Instance = value;
                    RaisePropertyChanged(nameof(MVM_Instance));
                }
            }
        }
        //=======================================================================================================================================================================//
        //MVM_Instance===========================================================================================================================================================//
        //=======================================================================================================================================================================//
        private string _config_Path;

        public string Config_Path
        {
            get
            {
                return _config_Path;
            }
            set
            {
                if(value != _config_Path)
                {
                    _config_Path = value;
                    RaisePropertyChanged(nameof(Config_Path));
                }
            }
        }
        //=======================================================================================================================================================================//
        //Collection_ImageModel_Left=============================================================================================================================================//
        //=======================================================================================================================================================================//
        private ObservableCollection<ImageModel> _collection_Bitmap_Config_Content;

        public ObservableCollection<ImageModel> Collection_ImageModel_Left
        {
            get
            {
                return _collection_Bitmap_Config_Content;
            }
            set
            {
                if(value != _collection_Bitmap_Config_Content)
                {
                    _collection_Bitmap_Config_Content = value;
                    RaisePropertyChanged(nameof(Collection_ImageModel_Left));
                }
            }
        }
        //=======================================================================================================================================================================//
        //Collection_ImageModel_Right============================================================================================================================================//
        //=======================================================================================================================================================================//
        private ObservableCollection<ImageModel> _collection_ImageModel_Right;

        public ObservableCollection<ImageModel> Collection_ImageModel_Right
        {
            get
            {
                return _collection_ImageModel_Right;
            }
            set
            {
                if(value != _collection_ImageModel_Right)
                {
                    _collection_ImageModel_Right = value;
                    RaisePropertyChanged(nameof(Collection_ImageModel_Right));
                }
            }
        }

        #endregion

        #region Commandes
        //=======================================================================================================================================================================//
        //Add_Command============================================================================================================================================================//
        //=======================================================================================================================================================================//
        private RelayCommand _add_Command;

        public RelayCommand Add_Command
        {
            get
            {
                //la vérification à été faites dans "Remove_Contents_Of_The_Config_Content_In_The_Images_Paths()" Mais il faut que "SelectedItemFolderTheme" ne soit pas null
                return _add_Command ?? (_add_Command = new RelayCommand(Add_To_Config_Content, Can_Add_To_Config_Content));
            }

        }
        //=======================================================================================================================================================================//
        //Remove_Command=========================================================================================================================================================//
        //=======================================================================================================================================================================//
        private RelayCommand _remove_Command;

        public RelayCommand Remove_Command
        {
            get
            {
                return _remove_Command ?? (_remove_Command = new RelayCommand(Remove_To_Config_Content, Can_Remove_To_Config_Content));
            }

        }
        //=======================================================================================================================================================================//
        //Close_Window_Command===================================================================================================================================================//
        //=======================================================================================================================================================================//
        private RelayCommand _close_Window_Command;

        public RelayCommand Close_Window_Command
        {
            get
            {
                
                return _close_Window_Command ?? (_close_Window_Command = new RelayCommand(Close_Window, null));
            }

        }

        private void Close_Window()
        {
            GC.Close_Window(MVM_Instance.CIW_Instance);
        }

        #endregion

        public Charged_Images_ViewModel()
        {

            Images_Paths = new ObservableCollection<string>();
            Config_Content = new ObservableCollection<string>();
            Collection_ImageModel_Left = new ObservableCollection<ImageModel>();
            Collection_ImageModel_Right = new ObservableCollection<ImageModel>();
            //Inscription 
            Messenger<MainViewModel>.Instance.Register(Groups.ViewModels, Get_MVM_Instance);

        }
        #region Fonctions d'action
        //=======================================================================================================================================================================//
        //Get_MVM_Instance=======================================================================================================================================================//
        //=======================================================================================================================================================================//
        private void Get_MVM_Instance(MainViewModel MVM)
        {
            //Si on arrive ici, c'est que la vue est affichée et donc que le message est bien passé

            //Je dois ajouter toutes les infos et pas juste affecter MVM.Images_Path car sinon les éléments de la liste ne sont jamais ajouté/retiré (vu que je passe la référence de la liste, je modifie la liste du MainViewModel, qui se remet direct à chaque fois)
            Images_Paths.Clear();
            Config_Content.Clear();
            Collection_ImageModel_Left.Clear();
            Collection_ImageModel_Right.Clear();

            foreach(string item in MVM?.Images_Paths)
            {
                Images_Paths.Add(item);
            }
            foreach(string item in MVM?.Config_Content)
            {
                Config_Content.Add(item); 
            }

            Theme_Path = MVM?.Theme_Path.ToString();
            Config_Path = MVM?.Config_Path.ToString();
            MVM_Instance = MVM;

            //Je retire toutes les informations similaire entre les 2 listes (pour pouvoir retirer/ajouter sans controler s'il y a des doublons)
            Remove_Contents_Of_The_Config_Content_In_The_Images_Paths();

            Create_Images();

            //Pour déplacer la fenêtre
            GC.Moving_Window(MVM_Instance.CIW_Instance);
        }

        private void Create_Images()
        {
            foreach(string item in Config_Content)
            {
                string path = Theme_Path + item;
                ImageModel img = new ImageModel(path, item);
                //Ajout dans la liste
                Collection_ImageModel_Left.Add(img); 
            }

            foreach(string item in Images_Paths)
            {
                string path = Theme_Path + item;
                ImageModel img = new ImageModel(path, item);
                //Ajout dans la liste
                Collection_ImageModel_Right.Add(img);
            }
        }

        //=======================================================================================================================================================================//
        //Remove_Contents_Of_The_Config_Content_In_The_Images_Paths==============================================================================================================//
        //=======================================================================================================================================================================//
        private void Remove_Contents_Of_The_Config_Content_In_The_Images_Paths()
        {//Je retire toutes les informations similaire entre les 2 listes (pour pouvoir retirer/ajouter sans controler si oui ou non c'est présent)
            Images_Paths = GF.Delete_Contents_Of_The_List2_In_The_List1(Images_Paths, Config_Content);
        }
        //=======================================================================================================================================================================//
        //Add_To_Config_Content==================================================================================================================================================//
        //=======================================================================================================================================================================//
        private void Add_To_Config_Content()
        {
            //1) supprimer de la liste "Images_Paths" l'élément sélectionné "SelectedItemFolderTheme"
            //Création d'une variable temporaire car quand on supprime, le "SelectedItemFolderTheme" devient null, et ça ajoute un élément null dans la liste lors du "Config_Content.Add"/"Collection_ImageModel_Left.Add"
            string tmp = SelectedItemFolderTheme.Name;
            ImageModel imgTmp = SelectedItemFolderTheme;

            Images_Paths.Remove(tmp);
            Collection_ImageModel_Right.Remove(imgTmp);

            //2) Ajouter à la liste 
            Config_Content.Add(tmp);
            Collection_ImageModel_Left.Add(imgTmp);

            SelectedItemFolderTheme = null;

            //On sauvegarde
            Save();
        }
        //=======================================================================================================================================================================//
        //Remove_To_Config_Content===============================================================================================================================================//
        //=======================================================================================================================================================================//
        private void Remove_To_Config_Content()
        {
            //1) supprimer de la liste "Config_Contnet" l'élément sélectionné "SelectedItemConfig"
            //Création d'une variable temporaire car quand on supprime, le "SelectedItemConfig" devient null, et ça ajoute un élément null dans la liste lors du "Images_Paths.Add"
            string tmp = SelectedItemConfig.Name;
            ImageModel imgTmp = SelectedItemConfig;

            Config_Content.Remove(tmp);
            Collection_ImageModel_Left.Remove(imgTmp);

            //2) Ajouter à la liste "Config_Content" l'élement sélectionné "SelectedItemFolderTheme"
            Images_Paths.Add(tmp);
            Collection_ImageModel_Right.Add(imgTmp);

            SelectedItemConfig = null;

            //On sauvegarde
            Save();
        }
        //=======================================================================================================================================================================//
        //Save===================================================================================================================================================================//
        //=======================================================================================================================================================================//
        private void Save()
        {
            GF.Write_String_IntoFile(Config_Content, Config_Path);
        }
        #endregion

        #region Fonctions de validation
        //=======================================================================================================================================================================//
        //Can_Add_To_Config_Content==============================================================================================================================================//
        //=======================================================================================================================================================================//
        private bool Can_Add_To_Config_Content()
        {
            return SelectedItemFolderTheme == null ? false : true;
        }
        //=======================================================================================================================================================================//
        //Can_Remove_To_Config_Content===========================================================================================================================================//
        //=======================================================================================================================================================================//
        private bool Can_Remove_To_Config_Content()
        {
            return SelectedItemConfig == null ? false : true;
        }
        #endregion
    }
}
