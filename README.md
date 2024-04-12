# Unity UI Framework
![image](https://github.com/DevsDaddy/UIFramework/assets/147835900/f8ddf1a0-fddd-44fb-af51-a863af6fca8b)
**Unity UI Framework** - A set of tools for interface developers, ready to use in your projects and supporting the **MVC/MVP/MVVVM** patterns. 
The basic architecture works based on **View and Layouts**, and interaction is handled by an **event system** to make your interface logic independent.

**Tested at Unity 2019.3+**

**Dependencies:**
- <a href="https://github.com/DevsDaddy/UnityEventFramework">Unity Event Framework</a> (included in project);
- Unity UI (UGUI);
- TextMesh Pro UGUI;

**Features:**
- Ready for MVP/MVC/MVVM;
- Views and Layouts loading from Scene / Resources;
- UI Controller for Views Management and Navigation;
- Binding / Unbinging Events;
- UI Particle System and UI Shaders;
- Simple Components Like Imaged Button and Web Image;
- Optimized for every platform;

## Get Started
**Unity UI Framework** is designed for your application and games using Unity UI (UGUI) and support Unity 2019+.

**Installation process:**
- Download and import <a href="https://github.com/DevsDaddy/UIFramework/releases">latest release from this page</a>;
- See <a href="#usage">usage examples below</a>;

**Or add this URL to UPM:**
```
https://github.com/DevsDaddy/UIFramework.git?path=/Assets/DevsDaddy/Shared/UIFramework/
```

**See Demo Scene to learn how it works (UIFrameworkDemo):**
![image](https://github.com/DevsDaddy/UIFramework/assets/147835900/a3904547-b1f3-433b-b783-4644972af605)

## How it works?

The **UI Framework** works according to the **View** / **Navigation** / **Layouts** scheme. And separates entities between them. 
Each **screen or window** is a **View** that is stored in history for navigation. Each View can have its own unlimited number of **Layouts**. 
**One of the View must be the home View for navigation**.

For more info see <a href="#usage">Usage Examples</a> or see **Demo Scene**.
Also I recommend to see <a href="https://github.com/DevsDaddy/OneUIKit">One UI Kit</a> based on UI Framework.

## Usage
**Views Binding:**
```csharp
UIFramework.BindView(Instantiate(homeView), true); // Home Page
UIFramework.BindView(Instantiate(pageView));       // Sub-Page
```

**Navigate to another view:**
```csharp
EventMessenger.Main.Publish(new OnViewNavigated {
    View = typeof(DemoPageView),
    Display = new DisplayOptions { IsAnimated = true, Delay = 0f, Duration = 0.5f, Type = AnimationType.Fade },
    Data = new DemoPageView.Data {
        Title = "Demo Page",
        Content = pageContent
    }
});
```

## API Reference
The following describes the API for working with the basic methods of the UIFramework, as well as any of the View and Layout presented on the basis of them.

**UIFramework Class:**
<table>
  <tr>
    <th>Method</th>
    <th>Usage</th>
  </tr>
  <tr>
    <td>**LoadViewFromResources** (generic)</td>
    <td>Load View Prefab from Resources and Bind It</td>
  </tr>
  <tr>
    <td>**GetView** (generic)</td>
    <td>Get Binded View of Type</td>
  </tr>
  <tr>
    <td>**BindView** (generic)</td>
    <td>Bind view of type</td>
  </tr>
  <tr>
    <td>**UnbindView** (generic)</td>
    <td>Unbind View of Type</td>
  </tr>
  <tr>
    <td>**Navigate**</td>
    <td>Navigate to another view</td>
  </tr>
  <tr>
    <td>**GetCurrentView**</td>
    <td>Get Current opened view</td>
  </tr>
  <tr>
    <td></td>
    <td></td>
  </tr>
  <tr>
    <td>**GoBack**</td>
    <td>Close current view and show previewsly opened view</td>
  </tr>
  <tr>
    <td>**GoHome**</td>
    <td>Close Current view and open Home page</td>
  </tr>
  <tr>
    <td>**GetWrapper**</td>
    <td>Get Coroutine Wrapper of UI Container</td>
  </tr>
  <tr>
    <td>**GoHome**</td>
    <td>Close Current view and open Home page</td>
  </tr>
</table>

**Base View / IBaseView:**
<table>
  <tr>
    <th>Method</th>
    <th>Usage</th>
  </tr>
  <tr>
    <td>**SetAsGlobalView**</td>
    <td>Set view as interscenic</td>
  </tr>
  <tr>
    <td>**ShowView**</td>
    <td>Show View with animation or without</td>
  </tr>
  <tr>
    <td>**HideView**</td>
    <td>Hide View with animation or without</td>
  </tr>
  <tr>
    <td>**ToggleView**</td>
    <td>Toggle View with animation or without</td>
  </tr>
  <tr>
    <td>**IsVisible**</td>
    <td>Get View Visibility State</td>
  </tr>
  <tr>
    <td>**OnViewAwake**</td>
    <td>Awake method alternative for view</td>
  </tr>
  <tr>
    <td>**OnViewStart**</td>
    <td>Start method alternative for view</td>
  </tr>
  <tr>
    <td>**OnViewDestroy**</td>
    <td>OnDestroy method alternative for view</td>
  </tr>
</table>

## Join Community
- <a href="https://discord.gg/xuNTKRDebx">Discord Community</a>
- <a href="https://boosty.to/devsdaddy">Buy me a Beer (Boosty)</a>
