#include <iostream>
#include "Node.h"

using namespace std;

Node::Node(int data)
{
	Node::data = data;
	leftNode = NULL;
	rightNode = NULL;
}
Node::~Node(void)
{
}