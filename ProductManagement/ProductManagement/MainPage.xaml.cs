using Plugin.Media;
using Plugin.Media.Abstractions;
using ProductManagement.Models;
using System;
using System.Linq;
using Xamarin.Forms;

namespace ProductManagement
{
    public partial class MainPage : ContentPage
    {
        
        public string _ImageFile { get; set; }

        public MainPage()
        {
            InitializeComponent();
            

        }
       
        protected async override void OnAppearing()
        {
            //base.OnAppearing();

            //Get All Produits
            var produitList = await App.SQLiteDb.GetItemsAsync();
            if (produitList != null)
            {
                lstProduits.ItemsSource = produitList.ToList();
                

            }
        }
        private async void BtnAdd_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtName.Text))
            {
                Produit produit = new Produit()
                {
                    Name = txtName.Text,
                    Description = txtDescription.Text,
                    Price = Convert.ToInt32(txtPrice.Text),
                    Image = _ImageFile


                };

                //Add New Produit
                await App.SQLiteDb.SaveItemAsync(produit);
                txtName.Text = string.Empty;
                await DisplayAlert("Success", "Produit added Successfully", "OK");
                //Get All Produits
                var produitList = await App.SQLiteDb.GetItemsAsync();
                if (produitList != null)
                {
                    lstProduits.ItemsSource = produitList;
                }
            }
            else
            {
                await DisplayAlert("Required", "Please Enter name!", "OK");
            }
        }

        private async void BtnRead_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtProduitId.Text))
            {
                //Get Produit
                var produit = await App.SQLiteDb.GetItemAsync(Convert.ToInt32(txtProduitId.Text));
                if (produit != null)
                {
                    txtName.Text = produit.Name;
                    await DisplayAlert("Success", "Produit Name: " + produit.Name, "OK");
                }
            }
            else
            {
                await DisplayAlert("Required", "Please Enter ProduitID", "OK");
            }
        }

        private async void BtnUpdate_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtProduitId.Text))
            {
                Produit produit = new Produit()
                {
                    Id = Convert.ToInt32(txtProduitId.Text),
                    Name = txtName.Text
                };

                //Update Produit
                await App.SQLiteDb.SaveItemAsync(produit);

                txtProduitId.Text = string.Empty;
                txtName.Text = string.Empty;
                await DisplayAlert("Success", "Produit Updated Successfully", "OK");
                //Get All Produits
                var produitList = await App.SQLiteDb.GetItemsAsync();
                if (produitList != null)
                {
                    lstProduits.ItemsSource = produitList;
                }

            }
            else
            {
                await DisplayAlert("Required", "Please Enter ProduitID", "OK");
            }
        }

        private async void BtnDelete_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtProduitId.Text))
            {
                //Get Produit
                var produit = await App.SQLiteDb.GetItemAsync(Convert.ToInt32(txtProduitId.Text));
                if (produit != null)
                {
                    //Delete Produit
                    await App.SQLiteDb.DeleteItemAsync(produit);
                    txtProduitId.Text = string.Empty;
                    await DisplayAlert("Success", "Produit Deleted", "OK");

                    //Get All Produits
                    var produitList = await App.SQLiteDb.GetItemsAsync();
                    if (produitList != null)
                    {
                        lstProduits.ItemsSource = produitList;
                    }
                }
            }
            else
            {
                await DisplayAlert("Required", "Please Enter ProduitID", "OK");
            }
        }



        private async void UploadImage_Clicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("Error", "Upload not supported on this device", "Cancel");
                return;
            }

            var file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
            {
                PhotoSize = PhotoSize.Full,
                CompressionQuality = 40
            });

            _ImageFile = file.Path;

            // Convert file to byre array, to bitmap and set it to our ImageView

            //byte[] imageArray = System.IO.File.ReadAllBytes(file.Path);
            //Bitmap bitmap = BitmapFactory.DecodeByteArray(imageArray, 0, imageArray.Length);
            //thisImageView.SetImageBitmap(bitmap);

        }

    }
}
