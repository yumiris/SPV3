using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using SPV3.Compiler.GUI.Annotations;
using SPV3.Installer;

namespace SPV3.Compiler.GUI
{
    public class Main : INotifyPropertyChanged
    {
        private ObservableCollection<Package> _packages = new ObservableCollection<Package>();
        private Package _selectedPackage = new Package();
        private string _output = string.Empty;

        public ObservableCollection<Package> Packages
        {
            get => _packages;
            set
            {
                if (Equals(value, _packages)) return;
                _packages = value;
                OnPropertyChanged();
            }
        }

        public Package SelectedPackage
        {
            get => _selectedPackage;
            set
            {
                if (Equals(value, _selectedPackage)) return;
                _selectedPackage = value;
                OnPropertyChanged();
            }
        }

        public string Output
        {
            get => _output;
            set
            {
                if (value == _output) return;
                _output = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void SavePackage()
        {
            Packages.Add(new Package
            {
                Name = SelectedPackage.Name,
                Description = SelectedPackage.Description,
                Directory = SelectedPackage.Directory,
                Files = SelectedPackage.Files
            });

            SelectedPackage = new Package();
        }

        public void SaveFile()
        {
            SelectedPackage.Files.Add(new File
            {
                Name = SelectedPackage.SelectedFile.Name,
                Description = SelectedPackage.SelectedFile.Description
            });

            SelectedPackage.SelectedFile = new File();
        }

        public void DeleteFile()
        {
            try
            {
                SelectedPackage.Files.Remove(SelectedPackage.Files.Single(i => i == SelectedPackage.SelectedFile));
                SelectedPackage.SelectedFile = new File();
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("Please select a file before deleting it.");
            }
        }

        public void DeletePackage()
        {
            try
            {
                Packages.Remove(Packages.Single(i => i == SelectedPackage));
                SelectedPackage = new Package();
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("Please select a package before deleting it.");
            }
        }

        public void SaveTo(string path)
        {
            try
            {
                var installer = new Installer.Installer
                {
                    Backup = (Backup) Guid.NewGuid().ToString(),

                    Packages = new Func<ObservableCollection<Package>, List<Installer.Package>>(x =>
                    {
                        var packages = new List<Installer.Package>();

                        foreach (var package in x)
                            packages.Add(new Installer.Package
                            {
                                Name = (Name) package.Name,
                                Description = (Description) package.Description,
                                Directory = (Directory) package.Directory,
                                Files = new Func<ObservableCollection<File>, List<Installer.File>>(y =>
                                {
                                    var files = new List<Installer.File>();

                                    foreach (var file in y)
                                        files.Add(new Installer.File
                                        {
                                            Name = (Name) file.Name,
                                            Description = (Description) file.Description
                                        });

                                    return files;
                                })(package.Files)
                            });

                        return packages;
                    })(_packages)
                };

                System.IO.File.WriteAllBytes(path, Persistence.ToBin(installer));
                Output = Persistence.ToXml(installer);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public class File : INotifyPropertyChanged
        {
            private string _description = string.Empty;
            private string _name = string.Empty;

            public string Name
            {
                get => _name;
                set
                {
                    if (value == _name) return;
                    _name = value;
                    OnPropertyChanged();
                }
            }

            public string Description
            {
                get => _description;
                set
                {
                    if (value == _description) return;
                    _description = value;
                    OnPropertyChanged();
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            [NotifyPropertyChangedInvocator]
            protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public class Package : INotifyPropertyChanged
        {
            private string _description = string.Empty;
            private string _directory = string.Empty;
            private ObservableCollection<File> _files = new ObservableCollection<File>();
            private string _name = string.Empty;
            private File _selectedFile = new File();

            public string Name
            {
                get => _name;
                set
                {
                    if (value == _name) return;
                    _name = value;
                    OnPropertyChanged();
                }
            }

            public string Description
            {
                get => _description;
                set
                {
                    if (value == _description) return;
                    _description = value;
                    OnPropertyChanged();
                }
            }

            public string Directory
            {
                get => _directory;
                set
                {
                    if (value == _directory) return;
                    _directory = value;
                    OnPropertyChanged();
                }
            }

            public ObservableCollection<File> Files
            {
                get => _files;
                set
                {
                    if (Equals(value, _files)) return;
                    _files = value;
                    OnPropertyChanged();
                }
            }

            public File SelectedFile
            {
                get => _selectedFile;
                set
                {
                    if (Equals(value, _selectedFile)) return;
                    _selectedFile = value;
                    OnPropertyChanged();
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            [NotifyPropertyChangedInvocator]
            protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}