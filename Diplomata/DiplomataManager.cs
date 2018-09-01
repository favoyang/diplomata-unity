using System.Collections.Generic;
using Diplomata.Helpers;
using Diplomata.Models;
using Diplomata.Persistence;
using Diplomata.Persistence.Models;
using UnityEngine;

namespace Diplomata
{
  /// <summary>
  /// The MonoBehaviour class to control all Diplomata data.
  /// </summary>
  [AddComponentMenu("")]
  [ExecuteInEditMode]
  public class DiplomataManager : MonoBehaviour
  {
    private static DiplomataData data;
    public static bool IsTalking;

    /// <summary>
    /// The Diplomata main data, all data come from here.
    /// </summary>
    /// <value>A static <seealso cref="Diplomata.DiplomataData">.</value>
    public static DiplomataData Data
    {
      get
      {
        if (data == null)
        {
          data = ScriptableObject.CreateInstance<DiplomataData>();
          data.ReadJSONs();
        }
        return data;
      }
    }

    /// <summary>
    /// Method to dispose the game data for new game.
    /// </summary>
    public static void DisposeData()
    {
      data = ScriptableObject.CreateInstance<DiplomataData>();
      data.ReadJSONs();
    }

    /// <summary>
    /// Get a global flag by name.
    /// </summary>
    /// <param name="name">The name of the flag.</param>
    /// <returns>The <seealso cref="Diplomata.Models.Flag"> object or null.</returns>
    public static Flag GetFlag(string name)
    {
      return Data.globalFlags.Find(Data.globalFlags.flags, name);
    }

    /// <summary>
    /// Get a character by his name.
    /// </summary>
    /// <param name="name">The character name.</param>
    /// <returns>The <seealso cref="Diplomata.Models.Character"> object or null.</returns>
    public static Character GetCharacter(string name)
    {
      return Character.Find(Data.characters, name);
    }

    /// <summary>
    /// Get a context by it's index.
    /// </summary>
    /// <param name="character">The character parent of the context.</param>
    /// <param name="contextIndex">The index of the context.</param>
    /// <returns>The <seealso cref="Diplomata.Models.Context"> object or null.</returns>
    public static Context GetContext(Character character, int contextIndex)
    {
      return Context.Find(character, contextIndex);
    }

    /// <summary>
    /// Get a context by name.
    /// </summary>
    /// <param name="talkable">The talkable to find in.</param>
    /// <param name="contextName">The context name.</param>
    /// <param name="language">The language of the name.</param>
    /// <returns>The context if found or null.</returns>
    public static Context GetContext(Character character, string contextName, string language)
    {
      return Context.Find(character, contextName, language);
    }

    /// <summary>
    /// Get a message by it's context and row index.
    /// </summary>
    /// <param name="context">The context parent of the message.</param>
    /// <param name="columnIndex">The index of the column.</param>
    /// <param name="rowIndex">The index of the row in the column.</param>
    /// <returns></returns>
    public static Message GetMessage(Context context, int columnIndex, int rowIndex)
    {
      var column = Column.Find(context, columnIndex);
      if (column == null) return null;
      var message = Message.Find(column.messages, rowIndex);
      if (message != null) return message;
      return null;
    }

    /// <summary>
    /// Get a context by it's unique id.
    /// </summary>
    /// <param name="uniqueId">The unique id (a guid string).</param>
    /// <returns>The <seealso cref="Diplomata.Models.Message"> object or null.</returns>
    public static Message GetMessage(string uniqueId)
    {
      foreach (Character character in Data.characters)
      {
        foreach (Context context in character.contexts)
        {
          foreach (Column column in context.columns)
          {
            var message = Message.Find(column.messages, uniqueId);
            if (message != null) return message;
          }
        }
      }
      return null;
    }

    /// <summary>
    /// Get a quest by it's name.
    /// </summary>
    /// <param name="name">The quest name.</param>
    /// <returns>The <seealso cref="Diplomata.Models.Quest"> object or null.</returns>
    public static Quest GetQuest(string name)
    {
      return Quest.Find(Data.quests, name);
    }

    /// <summary>
    /// Get a item by it's name.
    /// </summary>
    /// <param name="name">The item name.</param>
    /// <param name="language">The language of this name, if empty uses the options first language.</param>
    /// <returns>The <seealso cref="Diplomata.Models.Item"> object or null.</returns>
    public static Item GetItem(string name, string language = "")
    {
      if (language == "") language = Data.options.languages[0].name;
      return Item.Find(Data.inventory.items, name, language);
    }

    /// <summary>
    /// Get a item by it's id.
    /// </summary>
    /// <param name="itemId">The item id.</param>
    /// <returns>The item.</returns>
    public static Item GetItem(int itemId)
    {
      return Item.Find(data.inventory.items, itemId);
    }

    /// <summary>
    /// Return all characters persistent data.
    /// </summary>
    /// <returns>A array of characters persistent data.</returns>
    public static CharacterPersistentContainer GetPersistentCharacters()
    {
      return new CharacterPersistentContainer(Persistence.Data.GetArrayData<CharacterPersistent>(DiplomataManager.Data.characters.ToArray()));
    }

    /// <summary>
    /// Return all interactables persistent data.
    /// </summary>
    /// <returns>A array of interactables persistent data.</returns>
    public static InteractablePersistentContainer GetPersistentInteractables()
    {
      return new InteractablePersistentContainer(Persistence.Data.GetArrayData<InteractablePersistent>(DiplomataManager.Data.interactables.ToArray()));
    }

    /// <summary>
    /// Return all quests persistent data.
    /// </summary>
    /// <returns>A array of quests persistent data.</returns>
    public static QuestPersistentContainer GetPersistentQuests()
    {
      return new QuestPersistentContainer(Persistence.Data.GetArrayData<QuestPersistent>(DiplomataManager.Data.quests));
    }

    /// <summary>
    /// Return all talk logs persistent data.
    /// </summary>
    /// <returns>A array of talk logs persistent data.</returns>
    public static TalkLogPersistentContainer GetPersistentTalkLogs()
    {
      return new TalkLogPersistentContainer(Persistence.Data.GetArrayData<TalkLogPersistent>(DiplomataManager.Data.talkLogs));
    }

    /// <summary>
    /// Find a model by a field value.
    /// </summary>
    /// <param name="model">The desired Diplomata model.</param>
    /// <param name="field">The name of the field model.</param>
    /// <param name="value">The values to find.</param>
    /// <returns>A array of results.</returns>
    public static object[] Find(Model model, string field, object value)
    {
      switch (model)
      {
        case Model.OPTIONS:
          return (Options[]) Helpers.Find.In(new Options[] { Data.options }).Where(field, value).Results;

        case Model.CHARACTER:
          return (Character[]) Helpers.Find.In(Data.characters.ToArray()).Where(field, value).Results;

        case Model.CONTEXT:
          var contexts = new Context[0];
          foreach (var character in Data.characters)
          {
            contexts = ArrayHelper.Merge(contexts, (Context[]) Helpers.Find.In(character.contexts).Where(field, value).Results);
          }
          foreach (var interactable in Data.interactables)
          {
            contexts = ArrayHelper.Merge(contexts, (Context[]) Helpers.Find.In(interactable.contexts).Where(field, value).Results);
          }
          return contexts;

        case Model.COLUMN:
          var columns = new Column[0];
          foreach (var character in Data.characters)
          {
            foreach (var context in character.contexts)
            {
              columns = ArrayHelper.Merge(columns, (Column[]) Helpers.Find.In(context.columns).Where(field, value).Results);
            }
          }
          foreach (var interactable in Data.interactables)
          {
            foreach (var context in interactable.contexts)
            {
              columns = ArrayHelper.Merge(columns, (Column[]) Helpers.Find.In(context.columns).Where(field, value).Results);
            }
          }
          return columns;

        case Model.MESSAGE:
          var messages = new Message[0];
          foreach (var character in Data.characters)
          {
            foreach (var context in character.contexts)
            {
              foreach (var column in context.columns)
              {
                messages = ArrayHelper.Merge(messages, (Message[]) Helpers.Find.In(column.messages).Where(field, value).Results);
              }
            }
          }
          foreach (var interactable in Data.interactables)
          {
            foreach (var context in interactable.contexts)
            {
              foreach (var column in context.columns)
              {
                messages = ArrayHelper.Merge(messages, (Message[]) Helpers.Find.In(column.messages).Where(field, value).Results);
              }
            }
          }
          return messages;

        case Model.INTERACTABLE:
          return (Interactable[]) Helpers.Find.In(Data.interactables.ToArray()).Where(field, value).Results;

        case Model.ITEM:
          return (Item[]) Helpers.Find.In(Data.inventory.items).Where(field, value).Results;

        case Model.FLAG:
          return (Flag[]) Helpers.Find.In(Data.globalFlags.flags).Where(field, value).Results;

        case Model.QUEST:
          return (Quest[]) Helpers.Find.In(Data.quests).Where(field, value).Results;

        case Model.TALKLOG:
          return (TalkLog[]) Helpers.Find.In(Data.talkLogs).Where(field, value).Results;

        default:
          return new object[0];
      }
    }
  }
}