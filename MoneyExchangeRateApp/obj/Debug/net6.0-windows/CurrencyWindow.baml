   M S B A M L   `   `   `  ����O  KMoneyExchangeRateApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:MoneyExchangeRateAppMoneyExchangeRateApp  5   	   )    #MoneyExchangeRateApp.CurrencyWindow   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   4local"clr-namespace:MoneyExchangeRateApp     5   	     � Title$  CurrencyWindow��5   	   $	��450��6    $	��800��6-    � WindowStartupLocation$ CenterScreen=�6:   .��5	      � 
�������.+�5
   
   �� .+�5      �� -   5        $��spChangeBar��5   D   $��0 30 0 0q�6    �� Orientation$ 
Horizontal=�6+   $��Center=�6W   .+�5      �� -   5         ��Name$ 	txtNumber��5   5   $
��Left=�6    �� TextWrapping$
 Wrap=�6I   $��90��6^   �� 5      -   5        $��cbbSourceCurrencyName��5      $��80��68   $
��Left=�6C    �� DisplayMemberPath$
 Name��6^   5      �� 5      -   5         ��Name$ 	btnChange��5      $��	Calculate��6,   $��	10 0 10 0q�6@   6m   �� 5      -   5        $��lbDestinationValue��5      $��70��62   $
��Bold"�6=   6Q   �� 5      -   5        $ ��cbbDestinationCurrencyName��5      $	��120��6=   $
��Left=�6I   $
 Name��6d   5      �� 5      -   5        $��lbModeChange��5      $��50��6,   $
��Bold"�67   $
��Mode��6I   $��30 0 0 0q�6X   6l   �� 5      $��31��6   $ 
Horizontal=�6)   .+�5      	� -   5         	�Name$ rbCurrentNoSelectMode��5   <   $��CurrentNoSelect��6"    d� 	IsChecked$
 TrueY�6[   6l   	� 5      -	   5        $ rbCurrentSelectMode��5   ^   $��CurrentSelect��6"   $��10,0,0,0q�6:   ${�False��6L   6{   5      5      �� 5      -
   5        $��btbOpenChangeCurrency��5   R   $	��200��6   $��
0 10 0 110q�6!   $��Center=�65   $'��!Open currency conversion function��6o   6�   �� 5       $(��" Currency History Rate After Times��6   $
��Bold"�6A   $��20(�6S   
���**��6v   �� 5!      $��30��6   $ 
Horizontal=�6%   $��Right=�6>   $��0 0 0 10q�6Z   .+�5"      �� -   5        $ btnExportData��5"      $��ExportDataToFile��60   $��0 0 50 0q�6K   6{   �� 5#      $��Search��6   $��0 7 0 0q�6)   �� 5$      -   5        $ 	txtSearch��5$   6   $
��Left=�6   $
 Wrap=�6I   $	��100��6^   $��30��6j   $��2 0 20 0q�6v   6�   5%      �� 5&      -   5        	 ��Name$	 lvRateHistoryDTOList��5&      $
��Auto��65   $��Center=�6D   $��Center=�6a   
 �� View
 5'      �� 5(      .P�5)      �� $��ExchangeRateID��6)    �� Width$	 100��6A    �� DisplayMemberBinding 6M   ��  �� Path$ ExchangeRateId��� 5*      $��SourceCurrencyName��6)   $	 120��6E    6Q   �� $ CurrencySourceName��� 5+      $��DestinationCurrencyName��6)   $	 145��6J    6V   �� $ CurrencyTargetName��� 5,      $��DateTime��6)   $	 150��6;    6G   �� $
 Date� �� StringFormat$ dd/MM/yyyy HH:mm:ss���� 5-      $��SourceCurrencyPrice��6)   $	 120��6F    6R   �� $ SourceCurrencyPrice��� 5.      $��TargetCurrencyPrice��6)   $	 120��6F    6R   �� $ TargetCurrencyPrice�5/      51      53      54      55      