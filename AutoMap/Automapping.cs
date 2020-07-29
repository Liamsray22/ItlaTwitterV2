using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DataBase.Models;
using DataBase.ViewModels;

namespace AutoMap
{
    public class Automapping : Profile
    {
        public Automapping() {
            MapearRegistro();
            MapearLogin();
            MapearHome();
            MapearPublicaciones();
            MapearComentarios();
            MapearRespuesta();

        }

        private void MapearRespuesta()
        {
            CreateMap<RespuestaViewModel, Comentarios>().ReverseMap().
            ForMember(dest => dest.IdComentarioPadre, opt => opt.Ignore());
        }

        private void MapearRegistro () {
            CreateMap<RegistroViewModel, Usuarios>().ReverseMap().
            ForMember(dest=>dest.Foto, opt => opt.Ignore());

        }

        private void MapearLogin()
        {
            //CreateMap<LoginViewModel, Usuarios>().ReverseMap();
            //.ReverseMap()ForMember(dest=>dest.Campo, opt => opt.Ignore());

        }

        private void MapearHome()
        {
            //CreateMap<Publicaciones, PublicacionesViewModel>().ReverseMap();
            //.ReverseMap()ForMember(dest=>dest.Campo, opt => opt.Ignore());

        }

       
        private void MapearPublicaciones()
        {
            CreateMap<PublicacionesViewModel,Publicaciones>().ReverseMap()
            .ForMember(dest => dest.FotoPub, opt => opt.Ignore())
            .ForMember(dest => dest.Usuario, opt => opt.Ignore())
            .ForMember(dest => dest.publicaciones, opt => opt.Ignore())
            .ForMember(dest => dest.comentarios, opt => opt.Ignore())
            ;

        }

        private void MapearComentarios()
        {
            CreateMap<ComentariosViewModel,Comentarios >().ReverseMap()
            .ForMember(dest=>dest.Usuario, opt => opt.Ignore())
            .ForMember(dest => dest.comentarios2, opt => opt.Ignore());

        }
         

    }
}
