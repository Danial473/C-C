﻿using CC.Constants;
using CC.Models;
using CC.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;

namespace CC
{
    /// <summary>
    /// Interaction logic for BatchPage.xaml
    /// </summary>
    public partial class BatchPage : Page
    {
        public Array array { get; set; }

        public BatchPage()
        {
            InitializeComponent();
        }

        private void BatchIdTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string batchname = BatchIdTextBox.Text;

            if (batchname.StartsWith("|"))
            {
                batchname = batchname.Substring(1, batchname.Length - 1);
            }

            if (batchname.EndsWith("|"))
            {
                batchname = batchname.Substring(0, batchname.Length - 1);
            }

            var batchNumberParts = batchname.Split('|');
            if (batchNumberParts.Length == 5)
            {
                array = App.ArrayProvider.GetArrayByLIMArrayNumber(batchNumberParts[0]);

                if (array != null)
                {
                    AntigenGroupCombo.SelectedIndex = -1;

                    BatchNameTextBox.Text = $"{array.ArrayName} (Pts. {batchNumberParts[1]}-{batchNumberParts[2]})";
                    RundatePicker.SelectedDate = Convert.ToDateTime(batchNumberParts[3]);
                    BlockNumberTextBox.Text = batchNumberParts[4];

                    var groups = App.CCProvider.ArrayAntigens
                        .Where(a => a.ArrayId == array.ArrayId)
                        .Select(a => a.Group).Distinct()
                        .OrderBy(a=>a).ToList();

                    AntigenGroupCombo.ItemsSource = groups;
                    AntigenGroupCombo.Items.Refresh();
                    AntigenGroupCombo.SelectedIndex = 0;
                }
                else
                {
                    MessageBox.Show($"Array was not found. Coudn't match array number '{batchNumberParts[0]}' to any of database records");
                    BatchIdTextBox.Text = BatchNameTextBox.Text = string.Empty;
                }
            }
        }


        private void AntigenGroupCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AntigenGroupCombo.SelectedIndex > -1)
            {
                try
                {
                    var ExistingBatch = App.BatchProvider.GetBatchRecordsInGroup(
                       BatchNameTextBox.Text.Trim(),
                       RundatePicker.SelectedDate.Value,
                       Convert.ToInt32(BlockNumberTextBox.Text.Trim()),
                       Convert.ToString(AntigenGroupCombo.SelectedValue));

                    var selectedGroup = AntigenGroupCombo.SelectedItem as string;
                    var arrayAntigens = App.CCProvider.ArrayAntigens.Where(a => a.ArrayId == array.ArrayId && a.Group == selectedGroup).ToList();

                    var calibBatch = App.mapper.Map<List<BatchAntigen>>(arrayAntigens);
                    var negBatch = App.mapper.Map<List<BatchAntigen>>(arrayAntigens);
                    var posBatch = App.mapper.Map<List<BatchAntigen>>(arrayAntigens);

                    if (ExistingBatch != null && ExistingBatch.Any())
                    {
                        foreach (var batch in calibBatch)
                        {
                            batch.LotNumber = ExistingBatch.FirstOrDefault(b => b.AntigenId == batch.AntigenId && b.CCType == CCType.C.ToString())?.LotNumber;
                            batch.Type = CCType.C;
                        }
                        foreach (var batch in negBatch)
                        {
                            batch.LotNumber = ExistingBatch.FirstOrDefault(b => b.AntigenId == batch.AntigenId && b.CCType == CCType.N.ToString())?.LotNumber;
                            batch.Type = CCType.N;
                        }
                        foreach (var batch in posBatch)
                        {
                            batch.LotNumber = ExistingBatch.FirstOrDefault(b => b.AntigenId == batch.AntigenId && b.CCType == CCType.P.ToString())?.LotNumber;
                            batch.Type = CCType.P;
                        }
                    }
                    else
                    {
                        calibBatch.ForEach(a => a.Type = CCType.C);
                        negBatch.ForEach(a => a.Type = CCType.N);
                        posBatch.ForEach(a => a.Type = CCType.P);
                    }

                    CalibGid.ItemsSource = calibBatch;
                    NegGrid.ItemsSource = negBatch;
                    PosGrid.ItemsSource = posBatch;

                    CalibGid.Items.Refresh();
                    NegGrid.Items.Refresh();
                    PosGrid.Items.Refresh();
                }
                catch (Exception ex)
                {
                    if (ex.Data.Contains("logNumber"))
                    {
                        MessageBox.Show($"{ Messages.Exception} - log: {ex.Data["logNumber"]}");
                    }
                    else
                    {
                        var logNumber = Logger.Log(nameof(AntigenGroupCombo_SelectionChanged), new Dictionary<string, object>
                        {
                            { LogConsts.Exception, ex }
                        });

                        MessageBox.Show($"{ Messages.Exception} - log: {logNumber}");
                    }
                }
            }
        }

        private void ApplyAllBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(GenericNegLotTextBox.Text))
            {
                var negBatch = NegGrid.ItemsSource as List<BatchAntigen>;
                negBatch.ForEach(a => a.LotNumber = GenericNegLotTextBox.Text.Trim());

                NegGrid.Items.Refresh();
            }
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateForm())
            {
                ColorMapGrid.Visibility = Visibility.Hidden;
                var response = SaveBatch();
                if (string.IsNullOrEmpty(response))
                {
                    MessageBox.Show("Successfully saved");
                    CleanPage();
                }
                else
                {
                    MessageBox.Show(response, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                ColorMapGrid.Visibility = Visibility.Visible;
                MessageBox.Show("There are some invalid or missing fields on the page", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SaveAndNextBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateForm())
            {
                var response = SaveBatch();
                if (string.IsNullOrEmpty(response))
                {
                    if (AntigenGroupCombo.SelectedIndex < AntigenGroupCombo.Items.Count - 1) // there are still more groups to select from
                    {
                        MessageBox.Show("Successfully saved. Please click OK to load next antigen group");
                        AntigenGroupCombo.SelectedIndex++;
                    }
                    else
                    {
                        MessageBox.Show("Successfully saved. That was the last antigen group");
                        CleanPage();
                    }
                }
                else
                {
                    MessageBox.Show(response, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("There are some invalid or missing fields on the page", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private string SaveBatch()
        {
            var bachAntigens = new List<BatchAntigen>();

            bachAntigens.AddRange(NegGrid.ItemsSource as List<BatchAntigen>);
            bachAntigens.AddRange(PosGrid.ItemsSource as List<BatchAntigen>);
            bachAntigens.AddRange(CalibGid.ItemsSource as List<BatchAntigen>);

            var batch = new Batch
            {
                BatchName = BatchNameTextBox.Text.Trim(),
                RunDate = RundatePicker.SelectedDate.Value,
                BlockNumber = Convert.ToInt32(BlockNumberTextBox.Text.Trim()),
                AntigenGroup = AntigenGroupCombo.Text
            };

            return App.BatchProvider.UpsertBatch(batch, bachAntigens);
        }

        private bool ValidateForm()
        {
            bool isValid = true;

            if (string.IsNullOrWhiteSpace(BatchIdTextBox.Text))
            {
                BatchIdLabel.BorderBrush = Brushes.Red;
                BatchIdLabel.BorderThickness = new Thickness(2);
                isValid = false;
            }
            else
                BatchIdLabel.BorderThickness = new Thickness(0);

            if (string.IsNullOrWhiteSpace(BatchNameTextBox.Text))
            {
                BatchNameLbl.BorderBrush = Brushes.Red;
                BatchNameLbl.BorderThickness = new Thickness(2);
                isValid = false;
            }
            else
                BatchNameLbl.BorderThickness = new Thickness(0);

            if (string.IsNullOrWhiteSpace(BlockNumberTextBox.Text))
            {
                BlockNumberLabel.BorderBrush = Brushes.Red;
                BlockNumberLabel.BorderThickness = new Thickness(2);
                isValid = false;
            }
            else
                BlockNumberLabel.BorderThickness = new Thickness(0);


            if (!RundatePicker.SelectedDate.HasValue)
            {
                RunDateLabel.BorderBrush = Brushes.Red;
                RunDateLabel.BorderThickness = new Thickness(2);
                isValid = false;
            }
            else
                RunDateLabel.BorderThickness = new Thickness(0);

            var negBatch = NegGrid.ItemsSource as List<BatchAntigen>;
            var posBatch = PosGrid.ItemsSource as List<BatchAntigen>;
            var CalibBatch = CalibGid.ItemsSource as List<BatchAntigen>;

            if (negBatch != null && negBatch.Any())
            {
                if (!Convert.ToBoolean(AllowPartialCheckbox.IsChecked))
                {
                    isValid = negBatch.All(a => !string.IsNullOrWhiteSpace(a.LotNumber)) &&
                              posBatch.All(a => !string.IsNullOrWhiteSpace(a.LotNumber)) &&
                              CalibBatch.All(a => !string.IsNullOrWhiteSpace(a.LotNumber));
                    if (!isValid)
                    {
                        for (int i = 0; i < NegGrid.Items.Count; i++)
                        {
                            ContentPresenter negcell = NegGrid.Columns[1].GetCellContent(
                             (DataGridRow)NegGrid.ItemContainerGenerator.ContainerFromIndex(i)) as ContentPresenter;

                            var negLotNumbertxtBox = (negcell.ContentTemplate.FindName("LotNumberText", negcell) as TextBox);

                            if (string.IsNullOrWhiteSpace(negBatch[i].LotNumber))
                            {
                                negLotNumbertxtBox.BorderThickness = new Thickness(2);
                                negLotNumbertxtBox.BorderBrush = Brushes.Red;
                            }
                            else
                            {
                                negLotNumbertxtBox.BorderThickness = new Thickness(0);
                            }

                            ContentPresenter poscell = PosGrid.Columns[1].GetCellContent(
                                (DataGridRow)PosGrid.ItemContainerGenerator.ContainerFromIndex(i)) as ContentPresenter;

                            var posLotNumbertxtBox = (poscell.ContentTemplate.FindName("LotNumberText", poscell) as TextBox);

                            if (string.IsNullOrWhiteSpace(posBatch[i].LotNumber))
                            {
                                posLotNumbertxtBox.BorderThickness = new Thickness(2);
                                posLotNumbertxtBox.BorderBrush = Brushes.Red;
                            }
                            else
                            {
                                posLotNumbertxtBox.BorderThickness = new Thickness(0);
                            }


                            ContentPresenter calcell = CalibGid.Columns[1].GetCellContent(
                               (DataGridRow)CalibGid.ItemContainerGenerator.ContainerFromIndex(i)) as ContentPresenter;

                            var calLotNumbertxtBox = (calcell.ContentTemplate.FindName("LotNumberText", calcell) as TextBox);

                            if (string.IsNullOrWhiteSpace(CalibBatch[i].LotNumber))
                            {
                                calLotNumbertxtBox.BorderThickness = new Thickness(2);
                                calLotNumbertxtBox.BorderBrush = Brushes.Red;
                            }
                            else
                            {
                                calLotNumbertxtBox.BorderThickness = new Thickness(0);
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < NegGrid.Items.Count; i++)
                        {
                            NegGrid.Items.Refresh();
                            PosGrid.Items.Refresh();
                            CalibGid.Items.Refresh();

                            //(NegGrid.Items[i] as BatchAntigen).ShowExpired = Visibility.Hidden;
                            //(PosGrid.Items[i] as BatchAntigen).ShowExpired = Visibility.Hidden;
                            //(CalibGid.Items[i] as BatchAntigen).ShowExpired = Visibility.Hidden;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < NegGrid.Items.Count; i++)
                    {
                        if (string.IsNullOrWhiteSpace(negBatch[i].LotNumber) ||
                            string.IsNullOrWhiteSpace(posBatch[i].LotNumber) ||
                            string.IsNullOrWhiteSpace(CalibBatch[i].LotNumber))
                        {
                            if (!(string.IsNullOrWhiteSpace(negBatch[i].LotNumber) &&
                                  string.IsNullOrWhiteSpace(posBatch[i].LotNumber) &&
                                  string.IsNullOrWhiteSpace(CalibBatch[i].LotNumber)))
                            {
                                ContentPresenter negcell = NegGrid.Columns[1].GetCellContent(
                              (DataGridRow)NegGrid.ItemContainerGenerator.ContainerFromIndex(i)) as ContentPresenter;

                                var negLotNumbertxtBox = (negcell.ContentTemplate.FindName("LotNumberText", negcell) as TextBox);

                                if (string.IsNullOrWhiteSpace(negBatch[i].LotNumber))
                                {
                                    negLotNumbertxtBox.BorderThickness = new Thickness(2);
                                    negLotNumbertxtBox.BorderBrush = Brushes.Red;
                                }
                                else
                                {
                                    negLotNumbertxtBox.BorderThickness = new Thickness(0);
                                }

                                ContentPresenter poscell = PosGrid.Columns[1].GetCellContent(
                                    (DataGridRow)PosGrid.ItemContainerGenerator.ContainerFromIndex(i)) as ContentPresenter;

                                var posLotNumbertxtBox = (poscell.ContentTemplate.FindName("LotNumberText", poscell) as TextBox);

                                if (string.IsNullOrWhiteSpace(posBatch[i].LotNumber))
                                {
                                    posLotNumbertxtBox.BorderThickness = new Thickness(2);
                                    posLotNumbertxtBox.BorderBrush = Brushes.Red;
                                }
                                else
                                {
                                    posLotNumbertxtBox.BorderThickness = new Thickness(0);
                                }


                                ContentPresenter calcell = CalibGid.Columns[1].GetCellContent(
                                   (DataGridRow)CalibGid.ItemContainerGenerator.ContainerFromIndex(i)) as ContentPresenter;

                                var calLotNumbertxtBox = (calcell.ContentTemplate.FindName("LotNumberText", calcell) as TextBox);

                                if (string.IsNullOrWhiteSpace(CalibBatch[i].LotNumber))
                                {
                                    calLotNumbertxtBox.BorderThickness = new Thickness(2);
                                    calLotNumbertxtBox.BorderBrush = Brushes.Red;
                                }
                                else
                                {
                                    calLotNumbertxtBox.BorderThickness = new Thickness(0);
                                }

                                isValid = false;
                            }
                        }
                    }
                }

                // validate expiration of lot numbers 
                // Get existing calibrators, postitive controls, and negative controls that are not expired for selected array

                List<ActiveCCs> existingLotNumbers;

                if (Convert.ToBoolean(array.IsSubArray))
                {
                    existingLotNumbers = App.CCProvider.GetExistingCC(array.MasterArrayId);
                }
                else existingLotNumbers = App.CCProvider.GetExistingCC(array.ArrayId);

                for (int i = 0; i < NegGrid.Items.Count; i++)
                {
                    if (!string.IsNullOrEmpty(negBatch[i].LotNumber))
                    {
                        var negC = existingLotNumbers.FirstOrDefault(a => a.LotNumber == negBatch[i].LotNumber && a.AntigenId == negBatch[i].AntigenId && a.Type == CCType.N.ToString());
                         
                        ContentPresenter cell = NegGrid.Columns[1].GetCellContent(
                                (DataGridRow)NegGrid.ItemContainerGenerator.ContainerFromIndex(i)) as ContentPresenter;

                        var LotNumbertxtBox = (cell.ContentTemplate.FindName("LotNumberText", cell) as TextBox);

                        if (negC == null)
                        {
                            LotNumbertxtBox.BorderThickness = new Thickness(2);
                            LotNumbertxtBox.BorderBrush = Brushes.Blue;
                            isValid = false;
                        }
                        else if (negC.ExpirationDate < DateTime.Now)
                        {
                            LotNumbertxtBox.BorderThickness = new Thickness(2);
                            LotNumbertxtBox.BorderBrush = Brushes.Purple;
                            isValid = false;
                        }
                        else
                        {
                            LotNumbertxtBox.BorderThickness = new Thickness(0);
                        }
                    }

                    if (!string.IsNullOrEmpty(posBatch[i].LotNumber))
                    {
                        var posC = existingLotNumbers.FirstOrDefault(a => a.LotNumber == posBatch[i].LotNumber && a.AntigenId == posBatch[i].AntigenId && a.Type == CCType.P.ToString());

                        ContentPresenter cell = PosGrid.Columns[1].GetCellContent(
                              (DataGridRow)PosGrid.ItemContainerGenerator.ContainerFromIndex(i)) as ContentPresenter;

                        var LotNumbertxtBox = (cell.ContentTemplate.FindName("LotNumberText", cell) as TextBox);

                        if (posC == null)
                        {
                            LotNumbertxtBox.BorderThickness = new Thickness(2);
                            LotNumbertxtBox.BorderBrush = Brushes.Red;
                            isValid = false;
                        }
                        else if (posC.ExpirationDate < DateTime.Now)
                        {
                            LotNumbertxtBox.BorderThickness = new Thickness(2);
                            LotNumbertxtBox.BorderBrush = Brushes.Purple;
                            isValid = false;
                        }
                        else
                        {
                            LotNumbertxtBox.BorderThickness = new Thickness(0);
                        }
                    }

                    if (!string.IsNullOrEmpty(CalibBatch[i].LotNumber))
                    {
                        var calib = existingLotNumbers.FirstOrDefault(a => a.LotNumber == CalibBatch[i].LotNumber && a.AntigenId == CalibBatch[i].AntigenId && a.Type == CCType.C.ToString());
                        ContentPresenter cell = CalibGid.Columns[1].GetCellContent(
                               (DataGridRow)CalibGid.ItemContainerGenerator.ContainerFromIndex(i)) as ContentPresenter;

                        var LotNumbertxtBox = (cell.ContentTemplate.FindName("LotNumberText", cell) as TextBox);

                        if (calib == null)
                        {
                            LotNumbertxtBox.BorderThickness = new Thickness(2);
                            LotNumbertxtBox.BorderBrush = Brushes.Red;
                            isValid = false;
                        }
                        else if (calib.ExpirationDate < DateTime.Now)
                        {
                            LotNumbertxtBox.BorderThickness = new Thickness(2);
                            LotNumbertxtBox.BorderBrush = Brushes.Purple;
                            isValid = false;
                        }
                        else
                        {
                            LotNumbertxtBox.BorderThickness = new Thickness(0);
                        }
                    }
                }
            }

            return isValid;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            CleanPage();
        }

        private void CleanPage()
        {
            App.CCProvider.SetArrayAntigens();

            BatchIdTextBox.Text = BlockNumberTextBox.Text = GenericNegLotTextBox.Text = BatchNameTextBox.Text = string.Empty;
            RundatePicker.SelectedDate = null;
            AntigenGroupCombo.ItemsSource = null;
            AntigenGroupCombo.Items.Refresh();

            AllowPartialCheckbox.IsChecked = false;

            CalibGid.ItemsSource = NegGrid.ItemsSource = PosGrid.ItemsSource = null;
            CalibGid.Items.Refresh();
            NegGrid.Items.Refresh();
            PosGrid.Items.Refresh();

            BatchIdLabel.BorderThickness = RunDateLabel.BorderThickness = BlockNumberLabel.BorderThickness = new Thickness(0);
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            CleanPage();
            NavigationService.Content = null;
        }
    }
}
