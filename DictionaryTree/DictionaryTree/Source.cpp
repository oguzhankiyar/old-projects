#include <iostream>
#include "Tree.h"

using namespace std;

void menu();
Tree list;

void main()
{
	cout << "### Contacts Application with Trie ###" << endl;
	list.readData();
	menu();
}
void menu()
{
	int choice;
	cout << endl;
	cout << "1- Kayit ara" << "\t\t";
	cout << "4- Kayit guncelle" << endl;
	cout << "2- Kayitlari listele" << "\t";
	cout << "5- Kayit sil" << endl;
	cout << "3- Kayit ekle" << "\t\t";
	cout << "6- Cikis yap" << endl << endl;
	cout << "Seciminiz: ";
	cin >> choice;
	if (choice < 1 || choice > 6)
		menu();

	string tempWord, tempWord2;
	switch (choice)
	{
	case 1:
		cout << "Aranacak kelime: ";
		cin >> tempWord;
		list.searchName(tempWord);
		break;
	case 2:
		list.PreOrder();
		break;
	case 3:
		cout << "Eklenecek isim: ";
		cin >> tempWord;
		list.addName(tempWord);
		list.fillData();
		break;
	case 4:
		cout << "Guncellenecek isim: ";
		cin >> tempWord;
		cout << "Yeni isim: ";
		cin >> tempWord2;
		list.editName(tempWord, tempWord2);
		list.fillData();
		break;
	case 5:
		cout << "Silinecek isim: ";
		cin >> tempWord;
		list.deleteName(tempWord);
		list.fillData();
		break;
	case 6:
		exit(0);
	default:
		break;
	}
	menu();
}