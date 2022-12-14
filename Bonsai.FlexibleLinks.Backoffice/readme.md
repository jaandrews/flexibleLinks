# Flexible Links - for Umbraco 10
This package adds a new data type named "Flexible Links" to the umbraco backoffice. It allows users to add multiple links, similar to the built in "Multi Url Picker" data type, but it can be extended to accomodate additional kinds of links for things like modals, anchor links, etc. Out of the box it supports the following link types.

- Anchor - Used to create hash based links like "#example" to link to sections of the current page.
- Content - Used to select content from umbraco content section.
- Media - Used to select media from umbraco media section.
- External - Used to manually enter urls

It can be configured to allow one or many links. The label portion of the control can also be removed, which is useful for cases where the link is not text.

### Adding new links types
To add a new picker, all you need to do is implement the IFlexibleLinkType interface, making sure that the Key guid is unique to the picker. There are also a couple base classes than can be used to speed up creation of the picker.

- BaseManualLinkType - This is used for links where the url is manually entered. It's used for the [Anchor](https://github.com/jaandrews/flexibleLinks/blob/v10/main/Bonsai.FlexibleLinks.Core/Types/AnchorLinkType.cs) and [External](https://github.com/jaandrews/flexibleLinks/blob/v10/main/Bonsai.FlexibleLinks.Core/Types/ExternalLinkType.cs) link types out of the box.
- BasePickedLinkType - This is for pickers where the user can choose content to link to. It's used by the [Content](https://github.com/jaandrews/flexibleLinks/blob/v10/main/Bonsai.FlexibleLinks.Core/Types/ContentLinkType.cs) and [Media](https://github.com/jaandrews/flexibleLinks/blob/v10/main/Bonsai.FlexibleLinks.Core/Types/MediaLinkType.cs) link types out of the box. Note that the picker type determines how this will behave.
    - Content - Will use umbraco's built in content picker.
    - Media - Will use umbraco's built in media picker.
    - Custom - Will use a custom tree picker that uses the GetTree method of the picker type to pull in content. Note that this behavior can be changed to a customized implementation by setting the PickerPath of the type (by default this is set to "/app_plugins/flexiblelinks/tree-picker.html").

### Configuring the data type
![Initial Configuration](https://github.com/jaandrews/flexibleLinks/blob/v10/main/Bonsai.FlexibleLinks.Backoffice/images/data-type-init-creation.png)

There are 3 key pieces to the configuration of the Flexible Links data type.

- Allow Multiple - When active, this will allow multiple links to be selected.
- Disable Labels - This removes the label piece of the control. This is useful for when the links are images or icons rather than text.
- Pickers - This is a list of all the pickers that have been installed. By default they are disabled. When a picker is enabled, it will also reveal any settings configured in the Settings property of the picker, as shown below.

![Settings appear when picker enabled](https://github.com/jaandrews/flexibleLinks/blob/v10/main/Bonsai.FlexibleLinks.Backoffice/images/data-type-picker-settings.png)

### Using the data type
To use the Flexible Links control, just follow the steps below.

1. Click the button with the plus.
2. Choose the type of link you want to add.
3. Select the content you want to link to (this step is skipped if the type of the picker is "None").
4. Update the content in the link(s) as needed.