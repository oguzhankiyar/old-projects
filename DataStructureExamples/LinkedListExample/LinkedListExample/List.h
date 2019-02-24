#include <iostream>
#include "Node.h"

class List
{
public:
	List(void);
	~List(void);
	bool isEmpty(void);
	int findNode(double data);
	Node* insertNode(int index, double data);
	int deleteNode(double data);
	void displayList(void);
private:
	Node* head;
};

