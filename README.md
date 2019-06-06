
# Hands-On Mobile Development with .NET Core

<a href="Packt UTM URL of the Book"><img src="Cover Image URL of the Book" alt="Book Name" height="256px" align="right"></a>

This is the code repository for [Hands-On Mobile Development with .NET Core], published by Packt.

**Build cross-platform mobile applications with Xamarin, Visual Studio 2019, and .NET Core 3**

## What is this book about?
.NET Core is the general umbrella term used for Microsoft’s cross-platform toolset. Xamarin used for developing mobile applications, is one of the app model implementations for .NET Core infrastructure. 
In this book, you will learn how to design, architect, and develop highly attractive, maintainable, efficient, and robust mobile applications for multiple platforms, including iOS, Android, and UWP, with the toolset provided by Microsoft using Xamarin, .NET Core, and Azure Cloud Services. This book will take you through various phases of application development with Xamarin, from environment setup, design, and architecture to publishing, using real-world scenarios. Throughout the book, you will learn how to develop mobile apps using Xamarin, Xamarin.Forms and .NET Standard; implement a webbased backend composed of microservices with .NET Core using various Azure services including but not limited to Azure App Services, Azure Active Directory, Notification Hub, Logic Apps, and Azure Functions, Cognitive Services; create data stores using popular database technologies such as Cosmos DB, SQL and Realm.

This book covers the following exciting features: 
* Implement native applications for multiple mobile and desktop platforms
* Understand and use various Azure Services with .NET Core
* Make use of architectural patterns designed for mobile and web applications
* Understand the basic Cosmos DB concepts
* Understand how different app models can be used to create an app service

If you feel this book is for you, get your [copy](https://www.amazon.com/dp/1789538513) today!

<a href="https://www.packtpub.com/?utm_source=github&utm_medium=banner&utm_campaign=GitHubBanner"><img src="https://raw.githubusercontent.com/PacktPublishing/GitHub/master/GitHub.png" 
alt="https://www.packtpub.com/" border="5" /></a>


## Instructions and Navigations
The chapters dealing with Azure services would require an Azure subscription to be
created. With current subscription model, when an Azure free subscription is created, you
can get $200 credit for premium services, 12 months of free usage for certain services and a
set of services are free without limitation.
.NET Core and Xamarin development chapters would require an IDE such as Visual Studio
2019 and/or Visual Studio for Mac. Both of these applications are available with a freemium
model. In other words, the community edition for both applications are available with
certain limitations. Visual Studio code can be an open source alternative for these IDEs
when dealing with Azure service implementations.
If the development environment is setup on a macOS workstation, then Windows VM
might be required for certain development and deployment scenarios. While alternatives
are listed above, additional options are available for these type of a setup. Both of the
alternatives (that is, Parallels and VMWare) have trial versions available.
Online SaaS product offers such as Azure DevOps and Visual Studio App centre are also
freemium products. Nevertheless, the feature set offered with the free subscription should
be enough for the majority of the samples outlined in this book.

All of the code is organizes into two projects. That is Client and Web

The code will look like the following:
```
namespace FirstXamarinFormsApplication
{
  public partial class MainPage : ContentPage
  {
    public MainPage()
    {
      InitializeComponent();
      BindingContext = new MainPageViewModel();
    }
  }
}
```


**Following is what you need for this book:**
Copy and paste the Audience section from the EPIC.

With the following software and hardware list you can run all code files present in the book (Chapter 1-15).

### Software and Hardware List

| Chapter      | Software required                   | OS required              | Hardware specifications | Free/Proprietary
| -------------| ------------------------------------| -------------------------|-------------------------|
| All          | Xamarin Development Tools           | Windows                  | System with 16GB RAM    | 
| All          | Xamarin Development Tools           | Mac OS X                 | System with 16GB RAM    | 
| All(Optional)| VMWare                              | Mac OS X                 | System with 16GB RAM    | 88,95 Euros
| All(Optional)| Parallels                           | Mac OS X                 | System with 16GB RAM    | $79.99
| All          | XCode                               | Mac OS X                 |                         |
| 6            | Rstudio Desktop 0.99.903            | Windows, Mac OS X, and Linux (Any)|
| 7            | Rstudio Desktop 0.99.903            | Windows, Mac OS X, and Linux (Any) |
| 8            | Rstudio Desktop 0.99.903            | Windows, Mac OS X, and Linux (Any) |
| 9            | Rstudio Desktop 0.99.903            | Windows, Mac OS X, and Linux (Any) |
| 10           | Rstudio Desktop 0.99.903            | Windows, Mac OS X, and Linux (Any) |
| 11           | Rstudio Desktop 0.99.903            | Windows, Mac OS X, and Linux (Any) |
| 12           | Rstudio Desktop 0.99.903            | Windows, Mac OS X, and Linux (Any) |
| 13           | Rstudio Desktop 0.99.903            | Windows, Mac OS X, and Linux (Any) |
| 14           | Rstudio Desktop 0.99.903            | Windows, Mac OS X, and Linux (Any) |
| 15           | Rstudio Desktop 0.99.903            | Windows, Mac OS X, and Linux (Any) |


We also provide a PDF file that has color images of the screenshots/diagrams used in this book. [Click here to download it](Graphics Bundle Link).

### Related products <Paste books from the Other books you may enjoy section>
* Linux: Powerful Server Administration [[Packt]](https://www.packtpub.com/networking-and-servers/linux-powerful-server-administration?utm_source=github&utm_medium=repository&utm_campaign=9781788293778) [[Amazon]](https://www.amazon.com/dp/1788293770)

* Linux Device Drivers Development [[Packt]](https://www.packtpub.com/networking-and-servers/linux-device-drivers-development?utm_source=github&utm_medium=repository&utm_campaign=9781785280009) [[Amazon]](https://www.amazon.com/dp/1788293770)

## Get to Know the Author(s)
**Author Name**
Bio

**Author Name**
Bio


## Other books by the authors
* [Mastering Linux Network Administration](https://www.packtpub.com/networking-and-servers/mastering-linux-network-administration?utm_source=github&utm_medium=repository&utm_campaign=9781784399597)
* [Linux Mint Essentials](https://www.packtpub.com/networking-and-servers/linux-mint-essentials?utm_source=github&utm_medium=repository&utm_campaign=9781782168157)

### Suggestions and Feedback
[Click here](https://docs.google.com/forms/d/e/1FAIpQLSdy7dATC6QmEL81FIUuymZ0Wy9vH1jHkvpY57OiMeKGqib_Ow/viewform) if you have any feedback or suggestions.
