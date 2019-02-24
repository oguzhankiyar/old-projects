#include "Node.h"

class List
{
public:
	List(void);
	~List(void);
	bool isEmpty();
	Node* insertNode(int index, double data);	
	int findNode(double data);	
	int deleteNode(double data);
	void displayList(void);
private:
	Node* head;
	friend class Queue;
};