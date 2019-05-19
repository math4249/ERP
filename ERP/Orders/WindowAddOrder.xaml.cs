﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Application;
using Domain;

namespace ERP.Orders
{
    /// <summary>
    /// Interaction logic for WindowAddOrder.xaml
    /// </summary>
    public partial class WindowAddOrder : Window
    {
        private OrderRepository orderRepository = new OrderRepository();
        private List<Orderline> orderlines = new List<Orderline>();
        private Order order = new Order();
        private Orderline orderline = new Orderline();
        private OrderlineRepository orderlineRepository = new OrderlineRepository();
        private List<Object> tempList = new List<Object>();
        private ProductRepository productRepository = new ProductRepository();
        private Product product = new Product();
        private OfferRepository offerRepository = new OfferRepository();
        private List<Offer> offers = new List<Offer>();
        private Offer offer = new Offer();
        public WindowAddOrder()
        {
            InitializeComponent();
            WindowPickProduct.eventSendProduct += WindowPickProduct_eventSendProduct;
            WindowPickCustomer.eventSendList += WindowPickCustomer_eventSendList;
            UpdateTotalPrice();

            offers = offerRepository.DisplayOffers();
            for (int i = 0; i < offers.Count; i++)
            {
                ComboBoxOfferSelection.Items.Add($"{offers[i].OfferID} | {offers[i].Customer} | {offers[i].TotalPrice}");
            }
        }


        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            WindowShowDialog wsd = new WindowShowDialog();

            if (double.TryParse(TextBoxTotalPrice.Text, out double resultTotalPrice) && TextBoxDateOfPurchase.SelectedDate != null)
            {
                order.TotalPrice = double.Parse(TextBoxTotalPrice.Text);
                order.DateOfPurchase = DateTime.Parse(TextBoxDateOfPurchase.Text);
                orderRepository.AddOrder(order);

                for (int i = 0; i < orderlines.Count; i++)
                {
                    orderlines[i].OrderID = orderRepository.DisplayLastOrderID();
                    orderline.OrderID = orderlines[i].OrderID;
                    orderline.OfferID = 0;
                    orderline.Product = orderlines[i].Product;
                    orderline.Amount = orderlines[i].Amount;
                    orderlineRepository.AddOrderline(orderline);

                    product = orderline.Product;
                    product.ProductAmount -= orderline.Amount;
                    productRepository.EditProduct(product);

                }


                wsd.LabelShowDialog.Content = "Ordren blev tilføjet";
                wsd.ShowDialog();

                this.Close();
            }
            else
            {
                wsd.LabelShowDialog.Content = "Der var en fejl man";
                wsd.ShowDialog();
            }
        }

        private void ButtonAdd_Product_Click(object sender, RoutedEventArgs e)
        {
            WindowPickProduct wpp = new WindowPickProduct();
            wpp.ShowDialog();
        }

        void WindowPickProduct_eventSendProduct(Product item, double amount)
        {
            orderlines.Add(new Orderline(0, order.OrderID, 0, item, amount));
            
            Orderlines.ItemsSource = orderlines;

            Update();
        }

        void WindowPickCustomer_eventSendList(Domain.Customer items)
        {
            TextBoxCustomer.Text = items.CompanyName;
            order.Customer = items;
        }

        private void TextBoxCustomer_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            WindowPickCustomer wpc = new WindowPickCustomer();
            wpc.ShowDialog();
        }

        private void UpdateTotalPrice()
        {
            double totalPrice = 0;
            for (int i = 0; i < orderlines.Count; i++)
            {
                totalPrice += orderlines[i].Product.ProductPrice * orderlines[i].Amount;
            }
            TextBoxTotalPrice.Text = totalPrice.ToString();
        }

        private void Update() 
        {
            CollectionViewSource.GetDefaultView(Orderlines.ItemsSource).Refresh();
            UpdateTotalPrice();
        }

        private void ComboBoxOfferSelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string[] comboBoxOfferSelection = ComboBoxOfferSelection.SelectedItem.ToString().Split('|');
            TextBoxCustomer.Text = comboBoxOfferSelection[1].Substring(0, comboBoxOfferSelection[1].Length - 1);

            offer.OfferID = int.Parse(comboBoxOfferSelection[0].Substring(0, comboBoxOfferSelection[0].Length - 1));
            Orderlines.ItemsSource = orderlineRepository.DisplayOrderlines(new Order(), offer);
        }
    }
}