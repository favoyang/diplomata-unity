using System;
using System.Collections.Generic;
using Diplomata.Dictionaries;
using Diplomata.Helpers;
using Diplomata.Models;
using Diplomata.Persistence;
using Diplomata.Persistence.Models;
using UnityEngine;

namespace Diplomata.Models
{
  [Serializable]
  public class Talkable : Data
  {
    [SerializeField] private string uniqueId = Guid.NewGuid().ToString();
    public string name;
    public LanguageDictionary[] description;
    public Context[] contexts;

    [NonSerialized]
    public bool onScene;

    public Talkable() {}

    public Talkable(string name)
    {
      uniqueId = Guid.NewGuid().ToString();
      this.name = name;
      contexts = new Context[0];
      description = new LanguageDictionary[0];

      foreach (Language lang in DiplomataData.options.languages)
      {
        description = ArrayHelper.Add(description, new LanguageDictionary(lang.name, ""));
      }
    }

    /// <summary>
    /// Set the uniqueId if it is empty or null.
    /// </summary>
    /// <returns>Return a flag if it change or not.</returns>
    public bool SetId()
    {
      if (uniqueId == string.Empty || uniqueId == null)
      {
        uniqueId = Guid.NewGuid().ToString();
        return true;
      }
      return false;
    }

    /// <summary>
    /// Return the data of the object to save in a persistent object.
    /// </summary>
    /// <returns>A persistent object.</returns>
    public override Persistent GetData()
    {
      if (this.GetType() == typeof(Character))
      {
        var talkable = (Character) this;
        var talkablePersistent = new CharacterPersistent();
        talkablePersistent.id = uniqueId;
        talkablePersistent.influence = talkable.influence;
        talkablePersistent.contexts = Data.GetArrayData<ContextPersistent>(contexts);
        return talkablePersistent;
      }
      else if (this.GetType() == typeof(Interactable))
      {
        var talkable = (Interactable) this;
        var talkablePersistent = new InteractablePersistent();
        talkablePersistent.id = talkable.uniqueId;
        talkablePersistent.contexts = Data.GetArrayData<ContextPersistent>(talkable.contexts);
        return talkablePersistent;
      }
      else
      {
        var talkablePersistent = new TalkablePersistent();
        talkablePersistent.id = uniqueId;
        talkablePersistent.contexts = Data.GetArrayData<ContextPersistent>(contexts);
        return talkablePersistent;
      }
    }

    /// <summary>
    /// Store in a object data from persistent object.
    /// </summary>
    /// <param name="persistentData">The persistent data object.</param>
    public override void SetData(Persistent persistentData)
    {
      if (this.GetType() == typeof(Character))
      {
        var talkablePersistent = (CharacterPersistent) persistentData;
        uniqueId = talkablePersistent.id;
        ((Character) this).influence = talkablePersistent.influence;
        contexts = Data.SetArrayData<Context>(contexts, talkablePersistent.contexts);
      }
      else if (this.GetType() == typeof(Interactable))
      {
        var talkablePersistent = (InteractablePersistent) persistentData;
        uniqueId = talkablePersistent.id;
        contexts = Data.SetArrayData<Context>(contexts, talkablePersistent.contexts);
      }
      else
      {
        var talkablePersistent = (TalkablePersistent) persistentData;
        uniqueId = talkablePersistent.id;
        contexts = Data.SetArrayData<Context>(contexts, talkablePersistent.contexts);
      }
    }
  }
}