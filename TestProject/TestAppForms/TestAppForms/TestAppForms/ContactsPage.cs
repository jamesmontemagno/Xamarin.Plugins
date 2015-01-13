using Contacts.Plugin;
using Contacts.Plugin.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TestAppForms
{
  public class ContactsPage : ContentPage
  {

    ListView contactsList;
    public ContactsPage()
    {
      contactsList = new ListView();

      var cell = new DataTemplate(typeof(TextCell));

      cell.SetBinding(TextCell.TextProperty, "FirstName");
      cell.SetBinding(TextCell.DetailProperty, "LastName");

      contactsList.ItemTemplate = cell;

      contactsList.ItemSelected += (sender, args) =>
        {
          if (contactsList.SelectedItem == null)
            return;

          var contact = contactsList.SelectedItem as Contact;

          Navigation.PushAsync(new ContactPage(contact));

          contactsList.SelectedItem = null;
        };

      Content = contactsList;
    }
    bool loaded;
    protected async override void OnAppearing()
    {
      base.OnAppearing();

      if (loaded)
        return;

      loaded = true;

      if(await CrossContacts.Current.RequestPermission())
      {
        this.Title = "Loading...";
        this.IsBusy = true;
        List<Contact> contacts = null;
        CrossContacts.Current.PreferContactAggregation = false;
        await Task.Run(() =>
        {
          if(CrossContacts.Current.Contacts == null)
            return;

          contacts = CrossContacts.Current.Contacts
            .Where(c => !string.IsNullOrWhiteSpace(c.LastName) && c.Phones.Count > 0)
            .OrderBy(c => c.LastName)
            .ToList();
        });
        contactsList.ItemsSource = contacts;
        this.Title = "Contacts: " + contacts.Count;
        this.IsBusy = false;
      }
    }
  }

  public class ContactPage : ContentPage
  {
    public ContactPage(Contact contact)
    {
      var stack = new StackLayout();

      stack.Children.Add(new Label
        {
          Text = "DisplayName: " + contact.DisplayName
        });


      stack.Children.Add(new Label
      {
        Text = "FirstName: " + contact.FirstName
      });

      stack.Children.Add(new Label
      {
        Text = "MiddleName: " + contact.MiddleName
      });

      stack.Children.Add(new Label
      {
        Text = "LastName: " + contact.LastName
      });


      stack.Children.Add(new Label
      {
        Text = "Id: " + contact.Id
      });

      stack.Children.Add(new Label
      {
        Text = "Nickname: " + contact.Nickname
      });

      stack.Children.Add(new Label
      {
        Text = "Emails: "
      });

      foreach(var email in contact.Emails)
      {
        stack.Children.Add(new Label
        {
          Text = "Address: " + email.Address
        });


        stack.Children.Add(new Label
        {
          Text = "Label: " + email.Label
        });

        stack.Children.Add(new Label
        {
          Text = "Type: " + email.Type.ToString()
        });
      }

      stack.Children.Add(new Label
      {
        Text = "Addresses: "
      });

      foreach(var address in contact.Addresses)
      {
        stack.Children.Add(new Label
        {
          Text = "City: " + address.City
        });

        stack.Children.Add(new Label
        {
          Text = "Country: " + address.Country
        });


        stack.Children.Add(new Label
        {
          Text = "label: " + address.Label
        });


        stack.Children.Add(new Label
        {
          Text = "Post: " + address.PostalCode
        });


        stack.Children.Add(new Label
        {
          Text = "Region: " + address.Region
        });


        stack.Children.Add(new Label
        {
          Text = "StreetAddress: " + address.StreetAddress
        });


        stack.Children.Add(new Label
        {
          Text = "Type: " + address.Type
        });

      }

      stack.Children.Add(new Label
      {
        Text = "Prefix: " + contact.Prefix
      });


      stack.Children.Add(new Label
      {
        Text = "Suffix: " + contact.Suffix
      });

      
      stack.Children.Add(new Label
      {
        Text = "Notes: "
      });

      foreach(var note in contact.Notes)
      {
        stack.Children.Add(new Label
        {
          Text = "note: " + note.Contents
        });
      }

      stack.Children.Add(new Label
      {
        Text = "Organizations: "
      });

      foreach(var org in contact.Organizations)
      {
        stack.Children.Add(new Label
        {
          Text = "ContactTitle: " + org.ContactTitle
        });


        stack.Children.Add(new Label
        {
          Text = "Label: " + org.Label
        });


        stack.Children.Add(new Label
        {
          Text = "Name: " + org.Name
        });

        stack.Children.Add(new Label
        {
          Text = "Type: " + org.Type
        });
      }


      stack.Children.Add(new Label
      {
        Text = "Phones: "
      });

      foreach(var phone in contact.Phones)
      {
        stack.Children.Add(new Label
        {
          Text = "Label: " + phone.Label
        });

        stack.Children.Add(new Label
        {
          Text = "Number: " + phone.Number
        });

        stack.Children.Add(new Label
        {
          Text = "Type: " + phone.Type
        });
      }

      stack.Children.Add(new Label
      {
        Text = "Relationships: "
      });

      foreach(var relation in contact.Relationships)
      {
        stack.Children.Add(new Label
        {
          Text = "Name: " + relation.Name
        });

        stack.Children.Add(new Label
        {
          Text = "Type: " + relation.Type
        });
      }

      stack.Children.Add(new Label
      {
        Text = "Websites: "
      });

      foreach(var site in contact.Websites)
      {
        stack.Children.Add(new Label
        {
          Text = "Address: " + site.Address
        });
      }


      Content = new ScrollView
      {
        Content = stack
      };
    }
  }
}
