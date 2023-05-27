using System.Collections.Generic;

public class SelectableObjectsCollection : BaseCollection<IsleManager, List<SelectableObjectInfo>>
{
    private SelectableObjectInfo _currentSelectable;




    /// <summary>
    /// Adds a selectable object from the list to the collection for the specified IsleManager.
    /// </summary>
    /// <param name="key">The IsleManager key.</param>
    /// <param name="value">The selectable object within a list.</param>
    public override void Add(IsleManager key, List<SelectableObjectInfo> value)
    {
        _currentSelectable = value[0];

        if (Dict.ContainsKey(key))
        {
            // Check if the current selectable object is already in the list
            bool containsCurrentSelectable = Dict[key].Exists(selectable => selectable.Equals(_currentSelectable));

            if (containsCurrentSelectable)
            {
                // The selectable object is already in the list, return
                return;
            }

            // Add the current selectable object to the list
            Dict[key].Add(_currentSelectable);
        }
        else
        {
            // Add a new entry to the dictionary with the specified key and value
            Dict.Add(key, value);
        }
    }

    /// <summary>
    /// Removes a selectable object from the list within the collection for the specified IsleManager.
    /// </summary>
    /// <param name="key">The IsleManager key.</param>
    /// <param name="value">The selectable object within the list.</param>
    public override void Remove(IsleManager key, List<SelectableObjectInfo> value)
    {
        _currentSelectable = value[0];

        // Check if the dictionary contains the specified key and the selectable object is in the list
        bool containsBothKeyAndValue = Dict.ContainsKey(key) && Dict[key].Exists(selectable => selectable.Equals(_currentSelectable));

        if (containsBothKeyAndValue)
        {
            // Remove the selectable object from the list
            Dict[key].Remove(_currentSelectable);
        }
    }
}