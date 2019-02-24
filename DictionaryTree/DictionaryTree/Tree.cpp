#include <iostream>
#include <fstream>
#include "Tree.h"

using namespace std;

FILE* fPtr;

Tree::Tree()
{
	childCount = 0;
	isLast = false;
}
bool Tree::isEmpty()
{
	return childCount == 0;
}
void Tree::addName(string name)
{
	int charCount = name.length();
	Tree* parentTree = this;
	for (int i = 0; i < charCount; i++)
	{
		Tree* newTree = NULL;
		for (int j = 0; j < parentTree->childCount; j++)
		{
			if (parentTree->children[j]->data == name.at(i))
			{
				newTree = parentTree->children[j];
			}
		}
		if (newTree == NULL)
		{
			newTree = new Tree();
			newTree->data = name.at(i);
			newTree->isLast = i == charCount - 1 ? true : isLast;
			parentTree->children[parentTree->childCount++] = newTree;
			newTree->parent = parentTree;
			parentTree = newTree;
		}
		else
			parentTree = newTree;
	}
}
void Tree::PreOrder(Tree* head, string preChars)
{
	if (isEmpty())
		cout << "Rehbere henuz kayit eklenmemis." << endl;
	else
	{
		if (head == NULL)
			head = this;
		else
		{
			if (head->parent->childCount > 1 && head->parent->children[0] != head)
				cout << preChars << head->data;
			else
				cout << head->data;
		}
		if (head->isLast)
			cout << endl;
		for (int i = 0; i < head->childCount; i++)
		{
			PreOrder(head->children[i], head->childCount > 1 ? preChars.append(&head->data) : preChars);
		}
	}
}
void Tree::InOrder(Tree* head, string preChars)
{
	if (isEmpty())
		cout << "Rehbere henuz kayit eklenmemis." << endl;
	else
	{
		if (head == NULL)
			head = this;
		for (int i = 0; i < head->childCount / 2; i++)
		{
			InOrder(head->children[i], head->childCount > 1 ? preChars.append(&head->data) : preChars);
		}
		if (head != NULL)
		{
			if (head->parent->childCount > 1 && head->parent->children[0] != head)
				cout << preChars << head->data;
			else
				cout << head->data;
		}
		if (head->isLast)
			cout << endl;
		for (int i = head->childCount / 2; i < head->childCount; i++)
		{
			InOrder(head->children[i], head->childCount > 1 ? preChars.append(&head->data) : preChars);
		}
	}
}
void Tree::PostOrder(Tree* head, string preChars)
{
	if (isEmpty())
		cout << "Rehbere henuz kayit eklenmemis." << endl;
	else
	{
		if (head == NULL)
			head = this;
		for (int i = 0; i < head->childCount; i++)
		{
			PostOrder(head->children[i], head->childCount > 1 ? preChars.append(&head->data) : preChars);
		}
		if (head != NULL)
		{
			if (head->parent->childCount > 1 && head->parent->children[0] != head)
				cout << preChars << head->data;
			else
				cout << head->data;
		}
		if (head->isLast)
			cout << endl;
	}
}
void Tree::searchName(string name)
{
	Tree* newTree = NULL;
	if (!isEmpty())
	{
		Tree* parentTree = this;
		for (int i = 0; i < name.length(); i++)
		{
			if (parentTree != NULL)
			{
				for (int j = 0; j < parentTree->childCount; j++)
				{
					if (parentTree->children[j]->data == name.at(i))
					{
						newTree = parentTree->children[j];
					}
				}
				parentTree = newTree;
			}
		}
		if (newTree != NULL)
			PreOrder(newTree, name.substr(0, name.length() - 1));
	}
	if (newTree == NULL)
		cout << "Kelime ile eslesen kayit bulunamadi!" << endl;
}
bool Tree::deleteName(string name)
{
	Tree* curTree = this;
	int charCount = name.length();
	for (int i = 0; i < charCount; i++)
	{
		for (int j = 0; j < curTree->childCount; j++)
		{
			if (curTree->children[j]->data == name.at(i))
				curTree = curTree->children[j];
		}
	}
	Tree* lastTree = curTree;
	while (curTree != NULL && curTree != this)
	{
		if (curTree == lastTree || (curTree->childCount == 0 && !curTree->isLast))
		{
			if (curTree->childCount != 0)
			{
				curTree->isLast = false;
				break;
			}
			else
			{
				int childrenIndex = 0;
				for (int i = 0; i < curTree->parent->childCount; i++)
				{
					if (curTree->parent->children[i]->data == curTree->data)
					{
						childrenIndex = i;
						break;
					}
				}
				curTree->parent->children[childrenIndex] = NULL;
				for (int i = childrenIndex; i < curTree->parent->childCount; i++)
				{
					curTree->parent->children[i] = curTree->parent->children[i + 1];
				}
				curTree->parent->childCount--;
				curTree = curTree->parent;
			}
		}
		else
			break;
	}
	return true;
}
bool Tree::editName(string oldName, string newName)
{
	if (deleteName(oldName))
	{
		addName(newName);
		return true;
	}
	return false;
}
void Tree::fillData()
{
	fopen_s(&fPtr, "contacts.txt", "w");
	printData();
	fclose(fPtr);
}
void Tree::printData(Tree* head)
{
	if (head == NULL)
		head = this;
	else
		fprintf(fPtr, &head->data);
	if (head->isLast)
		fprintf(fPtr, "\n");
	for (int i = 0; i < head->childCount; i++)
	{
		printData(head->children[i]);
	}
}
void Tree::readData()
{
	string name = "";
	ifstream infile;
	infile.open("contacts.txt");
	while (!infile.eof())
	{
		getline(infile, name);
		addName(name);
	}
	infile.close();
}