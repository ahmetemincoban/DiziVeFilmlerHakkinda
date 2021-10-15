var app = new Vue({
    el:'#app',
    data:{
        kategoriler:[],
        yeniKategori:"",
        editing:null,
        catDel:null
    },
    methods:{
        listele:function(){
            fetch('https://localhost:5001/api/KategoriApi')
            .then(resp=>resp.json())
            .then(data=>{
                console.log(data);
                this.kategoriler=data;
            });
        },
        newCat:function(){
            fetch('https://localhost:5001/api/KategoriApi',{
                method:'POST',
                headers:{
                    'Content-Type':'application/json'
                },
                body: JSON.stringify({adi:this.yeniKategori}),
            })
            .then(resp=>resp.json())
            .then(data=>{
                console.log('Yeni Kategori Eklendi',data);
                this.listele();
                this.yeniKategori="";
            })
            .catch((error)=>{
                console.error('Kategori Eklenirken Bir Hata Oldu',error);
            });
        },
        delCat:function(kategori){
            fetch('https://localhost:5001/api/KategoriApi/'+kategori.id,{
                method:'DELETE',
                headers:{
                    'Content-Type':'application/json'
                },
            })
            .then(data=>{
                console.log('Kategori Başarıyla Silindi',data);
                this.listele();
            })
            .catch((error)=>{
                console.error('Kategori Silinirken Bir Hata Oldu',error);
            });
        },
        editCat:function(kategori){
            fetch('https://localhost:5001/api/KategoriApi/'+kategori.id,{
                method:'PUT',
                headers:{
                    'Content-Type':'application/json'
                },
                body: JSON.stringify(kategori),
            })
            .then(data=>{
                console.log('Kategori Sorunsuz Bir Şekilde Düzenlendi',data);
                this.listele();
            })
            .catch((error)=>{
                console.error('Kategori Düzenlenirken Bir Hata Oldu',error);
            });
        },
    },
    created:function(){
        this.listele();
    }
})