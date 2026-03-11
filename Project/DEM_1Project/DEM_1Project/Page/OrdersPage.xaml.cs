using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows.Controls;
using DEM_1Project.Model;

namespace DEM_1Project
{
    public partial class OrdersPage : Page
    {
        private List<Заказ> _allOrders;
        private string _currentSearch = "";
        private string _currentFilter = "";
        private string _currentSortField = "Дата";
        private bool _sortAscending = false; // по умолчанию по убыванию даты

        public OrdersPage()
        {
            InitializeComponent();
            cmbSortField.ItemsSource = new List<string> { "Номер", "Дата", "Статус", "Пользователь", "Товар" };
            cmbSortField.SelectedIndex = 1; // Дата
            rbDesc.IsChecked = true;
        }

        private void Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            using (var context = new HvostovDEMEntities())
            {
                _allOrders = context.Заказ
                                    .Include(o => o.Товар)
                                    .Include(o => o.Пользователь)
                                    .Include(o => o.Пункт_выдачи)
                                    .ToList();
            }

            // Уникальные производители товаров из заказов
            var manufacturers = _allOrders
                .Where(o => o.Товар != null && o.Товар.Производитель != null)
                .Select(o => o.Товар.Производитель)
                .Distinct()
                .OrderBy(m => m)
                .ToList();
            manufacturers.Insert(0, "Все");
            cmbFilter.ItemsSource = manufacturers;
            cmbFilter.SelectedIndex = 0;

            ApplyFilters();
        }

        private void Filter_Changed(object sender, EventArgs e)
        {
            _currentSearch = txtSearch.Text.ToLower();
            _currentFilter = cmbFilter.SelectedItem?.ToString();
            ApplyFilters();
        }

        private void Sort_Changed(object sender, EventArgs e)
        {
            _currentSortField = cmbSortField.SelectedItem?.ToString() ?? "Дата";
            _sortAscending = rbAsc.IsChecked == true;
            ApplyFilters();
        }

        private void ApplyFilters()
        {
            if (_allOrders == null) return;

            var query = _allOrders.AsEnumerable();

            // Фильтр по производителю товара
            if (!string.IsNullOrEmpty(_currentFilter) && _currentFilter != "Все")
                query = query.Where(o => o.Товар != null && o.Товар.Производитель == _currentFilter);

            // Текстовый поиск
            if (!string.IsNullOrEmpty(_currentSearch))
                query = query.Where(o =>
                    o.Номер.ToString().Contains(_currentSearch) ||
                    (o.Пользователь != null && o.Пользователь.ФИО != null && o.Пользователь.ФИО.ToLower().Contains(_currentSearch)) ||
                    (o.Товар != null && o.Товар.Артикул != null && o.Товар.Артикул.ToLower().Contains(_currentSearch)) ||
                    (o.Товар != null && o.Товар.Наименование_товара != null && o.Товар.Наименование_товара.ToLower().Contains(_currentSearch))
                );

            // Сортировка
            switch (_currentSortField)
            {
                case "Номер":
                    query = _sortAscending ? query.OrderBy(o => o.Номер) : query.OrderByDescending(o => o.Номер);
                    break;
                case "Дата":
                    query = _sortAscending ? query.OrderBy(o => o.Дата) : query.OrderByDescending(o => o.Дата);
                    break;
                case "Статус":
                    query = _sortAscending ? query.OrderBy(o => o.Статус) : query.OrderByDescending(o => o.Статус);
                    break;
                case "Пользователь":
                    query = _sortAscending ? query.OrderBy(o => o.Пользователь?.ФИО) : query.OrderByDescending(o => o.Пользователь?.ФИО);
                    break;
                case "Товар":
                    query = _sortAscending ? query.OrderBy(o => o.Товар?.Наименование_товара) : query.OrderByDescending(o => o.Товар?.Наименование_товара);
                    break;
                default:
                    query = _sortAscending ? query.OrderBy(o => o.Дата) : query.OrderByDescending(o => o.Дата);
                    break;
            }

            dgOrders.ItemsSource = query.ToList();
        }
    }
}