#include <stdio.h>
#include <stdlib.h>
#include <ctype.h>
#include <Windows.h>

typedef struct sehir{
	char sehirSembolu;
	int komsuSayisi;
	char *komsular;
	struct sehir *sonrakiSehir;
} Sehir;

int yolSayaci = 0;
char baslangicSehri, bitisSehri;
Sehir *sehirler;
char yol[17];
char sehirKontrol[17];
int kontrolIndex = 0;

void sehirleriEkle();
void sehirEkle(Sehir**, char, int, char*);
Sehir* sehirBul(char);
void yolBul(Sehir*, int);
void yolEkle(char, int);
void yolYazdir();
int gidildiMi(char);

void main(){
	sehirleriEkle();
	do{
		printf("Baslangic ve bitis sehirlerini giriniz: ");
		scanf("%c %c", &baslangicSehri, &bitisSehri);
		baslangicSehri = toupper(baslangicSehri);
		bitisSehri = toupper(bitisSehri);
		if (baslangicSehri == bitisSehri)
			printf("Uyari: Baslangic ve bitis sehirleri ayni olamaz! Tekrar deneyiniz..\n");
	} while (baslangicSehri == bitisSehri);
	printf("\n%c - %c icin bulunan yollar:\n-------------------------------------------------------------------------------\n", baslangicSehri, bitisSehri);
	yolBul(sehirBul(baslangicSehri), 0);
	printf("Toplamda %d yol bulundu.\n", yolSayaci);
	system("PAUSE");
}
void sehirEkle(char sehirSembolu, int komsuSayisi, char* komsular){
	Sehir* ilkSehir = (Sehir*)malloc(sizeof(Sehir));
	if (ilkSehir != NULL){
		ilkSehir -> sehirSembolu = sehirSembolu;
		ilkSehir -> komsuSayisi = komsuSayisi;
		ilkSehir -> komsular = komsular;
		ilkSehir -> sonrakiSehir = NULL;
		Sehir* geciciSehir = sehirler;
		Sehir* geciciSehir_2 = NULL;
		while(geciciSehir != NULL){
			geciciSehir_2 = geciciSehir;
			geciciSehir = geciciSehir -> sonrakiSehir;
		}
		if(geciciSehir_2 == NULL)
			sehirler = ilkSehir;
		else
			geciciSehir_2 -> sonrakiSehir = ilkSehir;
	}
}
void sehirleriEkle(){
	sehirEkle('A', 4, "BDFH");
	sehirEkle('B', 5, "ACIJR");
	sehirEkle('C', 4, "BDQR");
	sehirEkle('D', 5, "ACEPQ");
	sehirEkle('E', 4, "DFNP");
	sehirEkle('F', 5, "AEGMN");
	sehirEkle('G', 4, "FHLM");
	sehirEkle('H', 5, "AGIKL");
	sehirEkle('I', 4, "BHJK");
	sehirEkle('J', 2, "BI");
	sehirEkle('K', 2, "HI");
	sehirEkle('L', 2, "GH");
	sehirEkle('M', 2, "FG");
	sehirEkle('N', 2, "EF");
	sehirEkle('P', 2, "DE");
	sehirEkle('Q', 2, "CD");
	sehirEkle('R', 2, "BC");
}
Sehir* sehirBul(char sehirSembolu){
	Sehir* geciciSehir = sehirler;
	while (geciciSehir != NULL){
		if (geciciSehir -> sehirSembolu == sehirSembolu)
			return geciciSehir;
		geciciSehir = geciciSehir -> sonrakiSehir;
	}
}
int gidildiMi(char sehirSembolu){
	int i = 0;
	for (;i < sehirKontrol[i] != '\0';i++)
		if (sehirKontrol[i] == sehirSembolu)
			return 1;
	return 0;
}
void yolEkle(char sehirSembolu, int yolIndex){
	yol[yolIndex++] = sehirSembolu;
	yol[yolIndex] = '\0';
	sehirKontrol[kontrolIndex++] = sehirSembolu;
	sehirKontrol[kontrolIndex] = '\0';
}
void yolYazdir(){
	int i = 0;
	printf("%d. yol: ", ++yolSayaci);
	for (;i < yol[i+1] != '\0';i++){
		Sleep(100);
		printf("%c -> ", yol[i]);
	}
	printf("%c", yol[i]);
	printf("\n-------------------------------------------------------------------------------\n");
	sehirKontrol[0] = '\0';
}
void yolBul(Sehir* sehir, int yolIndex){
	//printf("%c -> ", sehir -> sehirSembolu);
	if (gidildiMi(sehir -> sehirSembolu) == 0){
		if (sehir -> sehirSembolu == bitisSehri){
			yolEkle(sehir -> sehirSembolu, yolIndex);
			yolYazdir();
		}
		else{
			yolEkle(sehir -> sehirSembolu, yolIndex);
			int i = 0, kontrol = 0;
			for (;i < sehir -> komsuSayisi;i++){
				Sehir* komsuSehir = sehirBul(sehir -> komsular[i]);
				if (gidildiMi(komsuSehir -> sehirSembolu) == 0){
					yolBul(komsuSehir, ++yolIndex);
					kontrol = 1;
				}
			}
			if (kontrol == 0){
				yolBul(sehirBul(yol[--yolIndex]), yolIndex);
			}
		}
	}
	else{
	}
}