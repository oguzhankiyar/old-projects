#include <string>
using namespace std;

class Tree
{
public:
	char data;
	int childCount;
	Tree* children[29];
	Tree* parent;
	bool isLast;

	Tree();
	bool isEmpty();

	void addName(string name);
	bool editName(string oldName, string newName);
	bool deleteName(string name);
	void searchName(string name);

	void PreOrder(Tree* head = NULL, string preChars = "");
	void InOrder(Tree* head = NULL, string preChars = "");
	void PostOrder(Tree* head = NULL, string preChars = "");
	void fillData();
	void readData();
private:
	void printData(Tree* head = NULL);
};