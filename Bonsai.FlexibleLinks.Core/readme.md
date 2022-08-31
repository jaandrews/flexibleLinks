# customPickers - for Umbraco 10
This is an umbraco package that contains only the core assemblies of the Flexible Links data type to allow users to create new link types without the app_plugin files.

### Adding new links types
To add a new picker, all you need to do is implement the IFlexibleLinkType interface, making sure that the Key guid is unique to the picker. There are also a couple base classes than can be used to speed up creation of the picker.

- BaseManualLinkType - This is used for links where the url is manually entered. It's used for the [Anchor](https://github.com/jaandrews/flexibleLinks/blob/v10/main/Bonsai.FlexibleLinks.Core/Types/AnchorLinkType.cs) and [External](https://github.com/jaandrews/flexibleLinks/blob/v10/main/Bonsai.FlexibleLinks.Core/Types/ExternalLinkType.cs) link types out of the box.
- BasePickedLinkType - This is for pickers where the user can choose content to link to. It's used by the [Content](https://github.com/jaandrews/flexibleLinks/blob/v10/main/Bonsai.FlexibleLinks.Core/Types/ContentLinkType.cs) and [Media](https://github.com/jaandrews/flexibleLinks/blob/v10/main/Bonsai.FlexibleLinks.Core/Types/MediaLinkType.cs) link types out of the box. Note that the picker type determines how this will behave.
    - Content - Will use umbraco's built in content picker.
    - Media - Will use umbraco's built in media picker.
    - Custom - Will use a custom tree picker that uses the GetTree method of the picker type to pull in content. Note that this behavior can be changed to a customized implementation by setting the PickerPath of the type (by default this is set to "/app_plugins/flexiblelinks/tree-picker.html").

