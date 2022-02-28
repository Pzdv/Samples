using FluentValidation.Results;
using Library_network.CustomEventArgs;
using Library_network.Enums;
using Library_network.Interfaces;
using Library_network.Models;
using Library_network.Reporters.Interfaces;
using Library_network.Serializers;
using Library_network.Validators;
using Library_network.Validators.Interfaces;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Schema;
using System.Xml.Xsl;

namespace Library_network.Library
{
    public class Library : ILibrary
    {
        private readonly ILogger _loadLogger = LogManager.GetLogger("LoadLogger");

        public event EventHandler<AddedItemEventArgs> FundAdded;
        public event EventHandler<DeletingItemEventArgs> FundDeleting;

        protected readonly ILibraryStorage _libraryStorage;

        public Library(ILibraryStorage libraryStorage)
        {
            _libraryStorage = libraryStorage;
        }

        public virtual void Add(LibraryItem item)
        {
            foreach (var storageItem in _libraryStorage.Items)
            {
                if (storageItem.Id == item.Id)
                    throw new InvalidOperationException("В библиотеке уже есть объект с таким Id");
            }
            _libraryStorage.Add(item);

            var eventArgs = new AddedItemEventArgs(item.GetType());
            OnFundAdded(eventArgs);
        }

        public virtual IEnumerable<LibraryItem> FindByName(string name)
        {
            return _libraryStorage.Items.Where(x => x.Name.Contains(name));
        }

        public virtual IEnumerable<Book> FindByAutor(string requiredAutor)
        {
            return _libraryStorage.Items
                .Where(x => x.GetType().Name == "Book")
                .Select(x => x as Book)
                .Where(x => x.Autors.Contains(requiredAutor));
        }

        public virtual IEnumerable<IGrouping<string, Book>> FindBooksByPublisher(string publisher)
        {
            return _libraryStorage.Items
                .Where(x => x.GetType().Name == "Book")
                .Select(x => x as Book)
                .Where(x => x.PublishingHouse.StartsWith(publisher))
                .GroupBy(x => x.PublishingHouse);
        }

        public virtual IEnumerable<IGrouping<int, LibraryItem>> GroupByPublishYear()
        {
            return _libraryStorage.Items.GroupBy(x => x.PublishYear);
        }

        public virtual void Remove(LibraryItem item)
        {
            var eventArgs = new DeletingItemEventArgs(item);
            OnFundDeleting(eventArgs);

            if (!eventArgs.Abort)
            {
                _libraryStorage.Remove(item);
            }
        }

        public virtual IEnumerable<LibraryItem> ShowAll()
        {
            return _libraryStorage.Items;
        }

        public virtual IEnumerable<LibraryItem> SortByPublishDate(bool ascending)
        {
            if (ascending)
            {
                return _libraryStorage.Items.OrderBy(x => x.PublishYear).ToList();
            }

            return _libraryStorage.Items.OrderByDescending(x => x.PublishYear).ToList();
        }

        public IOrderedEnumerable<TSource> Sort<TSource, TKey>(Func<TSource, TKey> keySelector) where TSource : LibraryItem
        {
            return _libraryStorage.Items.OfType<TSource>().OrderBy(keySelector);
        }

        public IEnumerable<T> FilterBy<T>(Func<T, bool> predicate) where T : LibraryItem
        {
            return _libraryStorage.Items.OfType<T>().Where(predicate);
        }

        public virtual void EditLibraryItem(LibraryItem item, string propertyName, object value)
        {
            var property = item.GetType().GetProperty(propertyName);
            var propValue = property?.GetValue(item);

            if (propValue != null && !propValue.Equals(value))
            {
                property.SetValue(item, value);
            }
        }

        public void Load(ILibraryValidator validator, string dataPath,  bool loadInvalidItems)
        {
            var dataManager = new DataManager();
            var warningMessageBuilder = new StringBuilder();

            var loadingItems = dataManager.GetData(dataPath);

            foreach (var item in loadingItems.Items)
            {
                var result = validator.Validate(item);
                if (loadInvalidItems)
                {
                    if (!result.IsValid)
                    {
                        CreateLoadWarningMessage(item, result, warningMessageBuilder);   
                    }
                }
                else
                {
                    if (!result.IsValid)
                    {
                        throw new InvalidOperationException(message: "Загружаемые данные содержат ошибки");
                    }
                }
            }
            _loadLogger.Warn(warningMessageBuilder.ToString());
            _libraryStorage.Items = loadingItems.Items;
        }

        public void LoadFromAnyXml(string xmlPath, string xsltPath)
        {
            XslTransform xslt = new();
            xslt.Load(xsltPath);

            var outputFile = CreateTemporaryFile();

            xslt.Transform(xmlPath, outputFile);

            Load(new LibraryFluentValidator(), outputFile, true);

            DeleteTemporaryFile(outputFile);
        }

        private static void DeleteTemporaryFile(string outputFile)
        {
            File.Delete(outputFile);
        }

        private static string CreateTemporaryFile()
        {
            var outputFile = Path.Combine(Environment.CurrentDirectory, "loadingData.xml");

            using (var newFile = File.Create(outputFile))
            {
                
            }

            return outputFile;
        }

        private void Settings_ValidationEventHandler(object sender, ValidationEventArgs e)
        {
            throw new Exception(message: $"Данные в XML файле не соответсвуют представленной схеме." +
                $"{Environment.NewLine}{e.Message}");
        }

        private static void CreateLoadWarningMessage(LibraryItem item, ValidationResult result, StringBuilder builder)
        {
            builder.Append($"\nid: {item.Id} Name: {item.Name}");
            foreach (var error in result.Errors)
            {
                builder.Append($"\n\t{error.PropertyName} {error.ErrorMessage}");
            }
        }

        public void Save(SerializeTo format, string path)
        {
            var dataManager = new DataManager();
            dataManager.SaveData(_libraryStorage, path, format);
        }

        public virtual void EditLibraryItem(LibraryItem item, IDictionary<string, object> propertiesValues)
        {
            if(!_libraryStorage.Items.Contains(item))
            {
                throw new InvalidOperationException(message: $"В каталоге нет объекта c ID = {item.Id} Name = {item.Name} ");
            }

            var itemHasAllProperties = ContainsAllProperies(item, propertiesValues);

            if (!itemHasAllProperties)
            {
                throw new ArgumentException(message: "Объект не содержит всех свойств", paramName: nameof(item));
            }

            foreach (var e in propertiesValues)
            {
                EditLibraryItem(item, e.Key, e.Value);
            }
        }

        public void CreateReport(string fileName, string path, IReporter reporter)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentException("Значение не можеть быть пустым", nameof(fileName));
            }

            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException("Значение не можеть быть пустым", nameof(path));
            }

            if (reporter == null)
            {
                throw new ArgumentNullException(nameof(reporter), "Значение не может быть null");
            }

            reporter.CreateReport(_libraryStorage.Items, path, fileName);
        }

        public IEnumerable<ValidationResult> Validate(ILibraryValidator validator)
        {
            return validator.Validate(_libraryStorage.Items); 
        }

        public static ValidationResult Validate(ILibraryValidator validator, LibraryItem item)
        {
            return validator.Validate(item);
        }

        protected virtual void OnFundAdded(AddedItemEventArgs eventArgs)
        {
            FundAdded?.Invoke(this, eventArgs);
        }

        protected virtual void OnFundDeleting(DeletingItemEventArgs eventArgs)
        {
            FundDeleting?.Invoke(this, eventArgs);
        }

        private static bool ContainsAllProperies(LibraryItem item, IDictionary<string, object> propertiesValues)
        {
            var properties = item.GetType().GetProperties().Select(prop => prop.Name);
            foreach (var property in propertiesValues.Keys)
            {
                if (!properties.Contains(property))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
