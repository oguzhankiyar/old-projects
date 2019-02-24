
/*
1- sag_geri 8
2- sol_ileri 9
3- sag_ileri 10
4- sol_geri 11
*/

#define SOL_GERI 11
#define SAG_GERI 8
#define SOL_ILERI 9
#define SAG_ILERI 10

void setup() {
  pinMode(SOL_GERI, OUTPUT);
  pinMode(SAG_GERI, OUTPUT);
  pinMode(SOL_ILERI, OUTPUT);
  pinMode(SAG_ILERI, OUTPUT);
}

void loop() {
  //digitalWrite(SOL_ILERI, HIGH);
  //digitalWrite(SAG_ILERI, HIGH);
}
