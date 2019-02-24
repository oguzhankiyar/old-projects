#include <stdio.h>
#include <stdlib.h>
#include <conio.h>
#include <time.h>
#include <math.h>

/*
	PROJE ADI:
		SANS OYUNLARI
	KODLAYAN:
		OĞUZHAN KİYAR
	KAYNAK:
		MILLIPIYANGO.GOV.TR
		EGE ÜNİVERSİTESİ BİLGİSAYAR MÜHENDİSLİĞİ PROGRAMLAMA PROJE ÖDEVİ
*/
//Sayilar[2][22] dizisinde 2 satir bulunmaktadir.. Ilk satir kullanicinin sayilarini, ikinci satir ise makinenin cektigi sayilari tutmaktadir..
void ana_menu() {
	int islem;
	char islem2;
	do {
		printf("\nANA MENU\n\n1. Sans Oyunlari\n2. Cikis\n\nSeciminizi Yapiniz: ");
		scanf("%d", &islem);
		switch(islem) {
			case 1:
				alt_menu();
				break;
			case 2:
				break;
		}
	} while(islem < 1 || islem > 3);
}
void alt_menu() {
	int islem;
	do {
		printf("\nSANS OYUNLARI\n\n1. Sayisal Loto Oynama\n2. Sans Topu Oynama\n3. On Numara Oynama\n4. Super Loto Oynama\n5. Ana Menu\n\nSeciminizi Yapiniz: ");
		scanf("%d", &islem);
		switch(islem) {
			case 1:
				sayisal_loto();
				break;
			case 2:
				sans_topu();
				break;
			case 3:
				on_numara();
				break;
			case 4:
				super_loto();
				break;
			case 5:
				ana_menu();
				break;
		}
	} while(islem < 1 || islem > 5);
}
void sayisal_loto() {
	int sayilar[2][22];
	char islem;
	printf("\nSAYISAL LOTO OYNAMA\n\n1 - 49 arasinda 6 sayi giriniz:\n");
	sayi_iste(sayilar, 6, 49);
	sayi_kontrol(sayilar, 6, 49);
	sayi_sirala(sayilar, 6, 6);
	kullanici_sayi_listele(sayilar, 6);
	makina_sayi_listele(sayilar, 6);
	int k = karsilastir(sayilar, 6, 6);
	//Tutan sayi adedini ve kazanilan odulu yazdiriyoruz
	printf("\n\nTutan sayi adedi: %d\n", k);
	if (k < 3)
		printf("\nODUL KAZANAMADINIZ\n");
	else
		printf("\nODUL: %d puan\n", (int)pow(2, k + 2));
	//Oyun bitti! Kullaniciya ne yapmak istedigini soruyoruz
	printf("\nYeni bir sayisal loto oynamak istiyor musunuz? (E/H)\n");
	scanf("%s", &islem);
	if (toupper(islem) == 'E')
		sayisal_loto();
	else
		alt_menu();
}
void sans_topu() {
	int sayilar[2][22], i;
	char islem;
	printf("\nSANS TOPU OYNAMA\n\n1 - 34 arasinda 5 sayi giriniz:\n");
	sayi_iste(sayilar, 5, 34);
	printf("1 - 14 arasinda 1 sayi giriniz:\n6. sayi: ");
	for (i=5;i<6;i++) {
		scanf("%d", &sayilar[0][5]);
		//Girilen sayi 1-14 arasinda degilse yeniden girmesini sagliyoruz
		if (sayilar[0][i] < 1 || sayilar[0][i] > 14) {
			printf("Girilen sayi 1 - 14 arasinda olmalidir! Yeniden giriniz..\n");
			i--;
		}
		sayilar[1][i] = rastgele_sayi(1, 14);
	}
	sayi_kontrol(sayilar, 5, 34);
	sayi_sirala(sayilar, 5, 5);
	kullanici_sayi_listele(sayilar, 5);
	printf("+ %d\n", sayilar[0][5]);
	makina_sayi_listele(sayilar, 5);
	printf("+ %d", sayilar[1][5]);
	int k = karsilastir(sayilar, 5, 5);
	int m = sayilar[0][5] == sayilar[1][5] ? 1 : 0;
	//Tutan sayi adedini ve kazanilan odulu yazdiriyoruz
	printf("\n\nTutan sayi adedi: %d + %d\n", k, m);
	if (k + m < 2 || (k == 2 && m == 0))
		printf("\nODUL KAZANAMADINIZ\n");
	else
		printf("\nODUL: %d puan\n", (int)pow(2, 2*k + m - 3));
	//Oyun bitti! Kullaniciya ne yapmak istedigini soruyoruz
	printf("\nYeni bir sans topu oynamak istiyor musunuz? (E/H)\n");
	scanf("%s", &islem);
	if (toupper(islem) == 'E')
		sans_topu();
	else
		alt_menu();
}
void on_numara() {
	int sayilar[2][22], i;
	char islem;
	printf("\nON NUMARA OYNAMA\n\n1 - 80 arasinda 10 sayi giriniz:\n");
	sayi_iste(sayilar, 10, 80);
	//Sayi_iste fonksiyonu ile 10 sayi istedik ve rastgele 10 sayi cektik.. Geriye kalan 12 sayiyi cekiyoruz..
	for (i=10;i<22;i++)
		sayilar[1][i] = rastgele_sayi(80);
	sayi_kontrol(sayilar, 22, 80);
	sayi_sirala(sayilar, 10, 22);
	kullanici_sayi_listele(sayilar, 10);
	makina_sayi_listele(sayilar, 22);
	int k = karsilastir(sayilar, 10, 22);
	//Tutan sayi adedini ve kazanilan odulu yazdiriyoruz
	printf("\n\nTutan sayi adedi: %d\n", k);
	if (k == 0)
		printf("\nODUL: 8 puan\n");
	else if (k < 6)
		printf("\nODUL KAZANAMADINIZ\n");
	else
		printf("\nODUL: %d puan\n", (int)pow(2, k - 2));
	//Oyun bitti! Kullaniciya ne yapmak istedigini soruyoruz
	printf("\nYeni bir on numara oynamak istiyor musunuz? (E/H)\n");
	scanf("%s", &islem);
	if (toupper(islem) == 'E')
		on_numara();
	else
		alt_menu();
}
void super_loto() {
	int sayilar[2][22];
	char islem;
	printf("\nSUPER LOTO OYNAMA\n\n1 - 54 arasinda 6 sayi giriniz:\n");
	sayi_iste(sayilar, 6, 54);
	sayi_kontrol(sayilar, 6, 54);
	sayi_sirala(sayilar, 6, 6);
	kullanici_sayi_listele(sayilar, 6);
	makina_sayi_listele(sayilar, 6);
	int k = karsilastir(sayilar, 6, 6);
	//Tutan sayi adedini ve kazanilan odulu yazdiriyoruz
	printf("\n\nTutan sayi adedi: %d\n", k);
	if (k < 3)
		printf("\nODUL KAZANAMADINIZ\n");
	else
		printf("\nODUL: %d puan\n", (int)pow(2, k + 2));
	//Oyun bitti! Kullaniciya ne yapmak istedigini soruyoruz
	printf("\nYeni bir super loto oynamak istiyor musunuz? (E/H)\n");
	scanf("%s", &islem);
	if (toupper(islem) == 'E')
		super_loto();
	else
		alt_menu();
}
void sayi_iste(int sayilar[2][22], int adet, int sinir) {
	//Kullanicidan sayilari girmesini istiyoruz
	int i, j;
	for (i=0;i<adet;i++) {
		printf("%d. sayi: ", i+1);
		scanf("%d", &sayilar[0][i]);
		//Girilen sayi 1-(sinir) arasinda degilse yeniden girmesini sagliyoruz
		if (sayilar[0][i] < 1 || sayilar[0][i] > sinir) {
			i--;
			printf("Girilen sayi 1 - %d arasinda olmalidir! Yeniden giriniz..\n", sinir);
		}
		else {
			for(j=0;j<i;j++)
				if (sayilar[0][j] == sayilar[0][i]) {
					printf("Ayni sayiyi birden fazla giremezsiniz! Yeniden giriniz..\n");
					i--;
				}
			sayilar[1][i] = rastgele_sayi(sinir);
		}
	}
}
void sayi_kontrol(int sayilar[2][22], int adet, int sinir) {
	//Eger ayni sayi onceden uretildiyse yeniden sayi urettiriyoruz
	int i, j;
	for (i=0;i<adet;i++) {
		for (j=0;j<i;j++) {
			if (sayilar[1][i] == sayilar[1][j]) {
				sayilar[1][i] = rastgele_sayi(sinir);
				i--;
				j=0;
			}
		}
	}
}
int rastgele_sayi(int sinir) {
	srand(time(NULL));
	return (rand() % sinir + 1);
}
int sayi_sirala(int sayilar[2][22], int adet1, int adet2) {
	int i, j;
	for (i=0;i<adet1;i++)
		for (j=0;j<adet1;j++)
			if (sayilar[0][i] > sayilar[0][j] && i<j) {
				int sayi = sayilar[0][i];
				sayilar[0][i] = sayilar[0][j];
				sayilar[0][j] = sayi;
			}
	for (i=0;i<adet2;i++)
		for (j=0;j<adet2;j++)
			if (sayilar[1][i] > sayilar[1][j] && i<j) {
				int sayi = sayilar[1][i];
				sayilar[1][i] = sayilar[1][j];
				sayilar[1][j] = sayi;
			}
}
int kullanici_sayi_listele(int sayilar[2][22], int adet) {
	//Kullanicinin girdigi sayilari yazdiriyoruz
	int i;
	printf("\nSizin sayilariniz: \n");
	for(i=0;i<adet;i++) {
		printf("%d ", sayilar[0][i]);
	}
}
int makina_sayi_listele(int sayilar[2][22], int adet) {
	//Makinanin cektigi sayilari yazdiriyoruz
	int i;
	printf("\n\nCekilen sayilar: \n");
	for(i=0;i<adet;i++) {
		printf("%d ", sayilar[1][i]);
	}
}
int karsilastir(int sayilar[2][22], int eleman_sayisi1, int eleman_sayisi2) {
	//Sayilari karsilastiriyoruz, tutan rakam sayisini hesapliyoruz
	int i, j;
	int k=0;
	for (i=0;i<eleman_sayisi1;i++)
		for (j=0;j<eleman_sayisi2;j++)
			if (sayilar[0][i] == sayilar[1][j])
				k++;
	return k;
}
int main(void) {
	printf("### Sans Oyunlari Uygulamasina Hosgeldiniz ###\nNOT: Bu oyunu yazan cocuk miyop oldu..!\n");
	ana_menu();
	return 0;
}
