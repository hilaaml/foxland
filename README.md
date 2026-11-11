# Foxland

Game 2D side-scrolling platformer, dibuat dengan Unity. Kamu jadi Finn, seekor rubah penjaga hutan yang harus mengambil kembali permata yang dicuri penyihir Shadow.

Coba main di [itch.io](https://hilarrity.itch.io/foxland). Gratis.

## Gameplay

Ada dua level. Di tiap level kamu harus mengumpulkan 5 gem untuk membuka portal ke level berikutnya.

- **Level 1, hutan cerah.** Platform kayu bertingkat, cherry cukup banyak, kesulitan mudah sampai normal.
- **Level 2, hutan musim dingin.** Area salju dan es, musuh lebih banyak, cherry lebih sedikit. Di sini kamu ketemu Shadow sebagai final boss.

Finn punya bar energi. Makan cherry menambah energi 25%. Kalau energi habis, geraknya jadi lambat dan musuh makin susah dihindari. Finn punya 5 nyawa, dan kalau semuanya habis kamu mengulang dari Level 1.

## Kontrol

| Tombol | Aksi |
| ------ | ---- |
| A / ← | Jalan ke kiri |
| D / → | Jalan ke kanan |
| W / ↑ / Space | Lompat |
| Esc / P | Pause |
| Enter | Konfirmasi menu |

## Dibuat dengan

- Unity (C#)
- Aset visual Sunny Land
- Build untuk Windows dan WebGL

## Struktur folder

```
Assets/            scene, script, sprite, audio
BuildWeb/          hasil build WebGL
Packages/          dependensi Unity
ProjectSettings/   pengaturan project
Icon.png, Icon.xcf ikon game (xcf = file sumber GIMP)
Title.png          gambar judul
Source.txt         sumber ikon
```

## Kredit

- Aset visual: Sunny Land
- Ikon: [fox](https://www.flaticon.com/free-icon/fox_5414810) dan [landscape](https://www.flaticon.com/free-icon/landscape_19037079) dari Flaticon
