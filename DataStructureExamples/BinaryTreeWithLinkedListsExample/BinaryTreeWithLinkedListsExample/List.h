#include <iostream>
#include "Node.h"

using namespace std;
class List
{
public:
	List(void);
	~List(void);
	void addNode(int data, Node* kok = NULL);
	bool editNode(int oldData, int newData);
	bool deleteNode(int data);
	void listNode(Node* kok = NULL);
	Node* findNode(int data);
	Node* findMin(Node* kok = NULL);
	Node* findMax(Node* kok = NULL);

	Node* head;
};

