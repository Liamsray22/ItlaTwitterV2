using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DataBase.Models;
using DataBase.ViewModels;
using DTO.DTO;

namespace AutoMap
{
    public class APIAutomapping : Profile
    {
        public APIAutomapping()
        {
            MapearRegistro();
            MapearAmigos();
            MapearHome();
            MapearPublicaciones();
            MapearComentarios();
            MapearRespuesta();

        }

        private void MapearRespuesta()
        {
            CreateMap<RespuestaViewModel, Comentarios>().ReverseMap().
            ForMember(dest => dest.Manda, opt => opt.Ignore()).
            ForMember(dest => dest.IdComentarioPadre, opt => opt.Ignore());
        }

        private void MapearRegistro()
        {
            CreateMap<RegistroViewModel, Usuarios>().ReverseMap().
            ForMember(dest => dest.Foto, opt => opt.Ignore());

        }

        private void MapearAmigos()
        {
            CreateMap<Usuarios, ListaAmigosDTO>().ReverseMap()
            .ForMember(dest => dest.Telefono, opt => opt.Ignore())
            .ForMember(dest => dest.Correo, opt => opt.Ignore())
            .ForMember(dest => dest.Activo, opt => opt.Ignore())
            .ForMember(dest => dest.Clave, opt => opt.Ignore())
            .ForMember(dest => dest.AmigosIdAmigoNavigation, opt => opt.Ignore())
            .ForMember(dest => dest.AmigosIdUsuarioNavigation, opt => opt.Ignore());



        }

        private void MapearHome()
        {
            //CreateMap<Publicaciones, PublicacionesViewModel>().ReverseMap();
            //.ReverseMap()ForMember(dest=>dest.Campo, opt => opt.Ignore());

        }


        private void MapearPublicaciones()
        {
            CreateMap<PublicacionesDTO, Publicaciones>().ReverseMap()
            .ForMember(dest => dest.Usuario, opt => opt.Ignore())
            .ForMember(dest => dest.comentarios, opt => opt.Ignore())
            ;

        }

        private void MapearComentarios()
        {
            CreateMap<ComentariosDTO, Comentarios>().ReverseMap()
            .ForMember(dest => dest.Usuario, opt => opt.Ignore())
            .ForMember(dest => dest.respuestas, opt => opt.Ignore());

        }


    }
}
