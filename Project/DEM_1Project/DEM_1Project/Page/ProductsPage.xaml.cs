using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using DEM_1Project.Model;

namespace DEM_1Project
{
    public partial class ProductsPage : Page
    {
        private List<Товар> _allProducts;
        private string _currentSearch = "";
        private string _currentManufacturer = "";
        private string _currentSortField = "Наименование_товара";
        private bool _sortAscending = true;

        public ProductsPage()
        {
            InitializeComponent();
            cmbSortField.ItemsSource = new List<string> { "Наименование_товара", "Производитель", "Цена", "Артикул" };
            cmbSortField.SelectedIndex = 0;
            rbAsc.IsChecked = true;
        }

        private void Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            using (var context = new HvostovDEMEntities())
            {
                _allProducts = context.Товар.ToList();
            }
            // Уникальные производители для фильтра
            var manufacturers = _allProducts
                .Select(p => p.Производитель)
                .Distinct()
                .OrderBy(m => m)
                .ToList();
            manufacturers.Insert(0, "Все");
            cmbManufacturer.ItemsSource = manufacturers;
            cmbManufacturer.SelectedIndex = 0;

            ApplyFilters();
        }

        private void Filter_Changed(object sender, EventArgs e)
        {
            _currentSearch = txtSearch.Text.ToLower();
            _currentManufacturer = cmbManufacturer.SelectedItem?.ToString();
            ApplyFilters();
        }

        private void Sort_Changed(object sender, EventArgs e)
        {
            _currentSortField = cmbSortField.SelectedItem?.ToString() ?? "Наименование_товара";
            _sortAscending = rbAsc.IsChecked == true;
            ApplyFilters();
        }

        private void ApplyFilters()
        {
            if (_allProducts == null) return;

            var query = _allProducts.AsEnumerable();

            // Фильтр по производителю
            if (!string.IsNullOrEmpty(_currentManufacturer) && _currentManufacturer != "Все")
                query = query.Where(p => p.Производитель == _currentManufacturer);

            // Текстовый поиск
            if (!string.IsNullOrEmpty(_currentSearch))
                query = query.Where(p =>
                    (p.Наименование_товара?.ToLower().Contains(_currentSearch) == true) ||
                    (p.Артикул?.ToLower().Contains(_currentSearch) == true) ||
                    (p.Производитель?.ToLower().Contains(_currentSearch) == true) ||
                    (p.Описание?.ToLower().Contains(_currentSearch) == true)
                );

            // Сортировка
            switch (_currentSortField)
            {
                case "Наименование_товара":
                    query = _sortAscending ? query.OrderBy(p => p.Наименование_товара) : query.OrderByDescending(p => p.Наименование_товара);
                    break;
                case "Производитель":
                    query = _sortAscending ? query.OrderBy(p => p.Производитель) : query.OrderByDescending(p => p.Производитель);
                    break;
                case "Цена":
                    query = _sortAscending
                        ? query.OrderBy(p => Convert.ToDecimal(p.Цена))
                        : query.OrderByDescending(p => Convert.ToDecimal(p.Цена));
                    break;
                case "Артикул":
                    query = _sortAscending ? query.OrderBy(p => p.Артикул) : query.OrderByDescending(p => p.Артикул);
                    break;
                default:
                    query = _sortAscending ? query.OrderBy(p => p.Наименование_товара) : query.OrderByDescending(p => p.Наименование_товара);
                    break;
            }

            dgProducts.ItemsSource = query.ToList();
        }
    }
}