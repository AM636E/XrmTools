using System;
using System.Collections.Generic;
using Microsoft.Xrm.Sdk;

namespace DynamicaLabs.XrmTools.Data
{
    public interface IOptionSetRetriever
    {
        /// <summary>
        ///     Get optionset text from int value.
        /// </summary>
        /// <param name="optionSetName">Logical name of option set.</param>
        /// <param name="value">Value of option set.</param>
        /// <param name="entityLogicalName">etn.</param>
        /// <param name="default">Default value to return String.Empty when value.Value equals @default</param>
        /// <exception cref="ArgumentNullException">optionSetName is null</exception>
        /// <exception cref="ArgumentNullException">value is null</exception>
        /// <returns>Option set string value.</returns>
        string GetOptionSetText(string optionSetName, OptionSetValue value, string entityLogicalName, int @default = 0);

        /// <summary>
        ///     Get option set value from string.
        /// </summary>
        /// <param name="optionSetName"></param>
        /// <param name="optionSetString"></param>
        /// <param name="entityLogicalName"></param>
        /// <returns></returns>
        OptionSetValue GetOptionSetValue(string optionSetName, string optionSetString, string entityLogicalName);

        Dictionary<int, string> GetMapping(string entityName, string optionSetName); 
    }
}