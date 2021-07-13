using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.Constants;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Performance;
using Core.Aspects.Autofac.Transaction;
using Core.Aspects.Autofac.Validation;
using Core.Utilities.Business;
using Core.Utilities.Helpers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using GroupDocs.Conversion;
using GroupDocs.Conversion.FileTypes;
using GroupDocs.Conversion.Options.Convert;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Text;
using FluentValidation;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;



namespace Business.Concrete
{
    public class UserImageManager : IUserImageService
    {
        IUserImageDal _userImageDal;
        public string Server = "https://localhost:44349/";

        public UserImageManager(IUserImageDal userImageDal)
        {
            _userImageDal = userImageDal;
        }
        
        //[SecuredOperation("user,admin")]
        [CacheRemoveAspect("IUserImageService.Get")]
        [TransactionScopeAspect]
        public IDataResult<string> Add(IFormFile file,UserImage userImage)
        {
            
            userImage.ImagePath = FileHelper.AddAsync(file);
            

            string uniqueString  = Guid.NewGuid().ToString("N")
               + "_" + DateTime.Now.Month + "_"
               + DateTime.Now.Day + "_"
               + DateTime.Now.Year ;

            userImage.UserImageDate = DateTime.Now;
            if (userImage.ImagePath.ToLower().Contains(".png"))
            {
                userImage.ConvertType = "jpeg";
                IDataResult<string> result = Convert(userImage.ImagePath,userImage.ConvertType, uniqueString);
                if (result != null)
                {
                    userImage.ImagePath = "\\Image\\" + uniqueString + "." + userImage.ConvertType;
                    _userImageDal.Add(userImage);
                    return new SuccessDataResult<string>(result.Data, result.Message);
                }

            }
            else if (userImage.ImagePath.ToLower().Contains(".jpg"))
            {
                userImage.ConvertType = "png";
                IDataResult<string> result = Convert(userImage.ImagePath, userImage.ConvertType, uniqueString);
                if (result != null)
                {
                    userImage.ImagePath = "\\Image\\" + uniqueString + "." + userImage.ConvertType;
                    _userImageDal.Add(userImage);
                    return new SuccessDataResult<string>(result.Data, result.Message);
                }

            }
            else if (userImage.ImagePath.ToLower().Contains(".jpeg"))
            {
                userImage.ConvertType = "jpg";
                IDataResult<string> result = Convert(userImage.ImagePath, userImage.ConvertType, uniqueString);
                if (result != null)
                {
                    userImage.ImagePath = "\\Image\\" + uniqueString + "." + userImage.ConvertType;
                    _userImageDal.Add(userImage);
                    return new SuccessDataResult<string>(result.Data, result.Message);
                }
            }
            else
            {
                userImage.ConvertType = "webp";
                IDataResult<string> result = Convert(userImage.ImagePath, userImage.ConvertType, uniqueString);
                if (result != null)
                {
                    userImage.ImagePath = "\\Image\\" + uniqueString + "." + userImage.ConvertType;
                    _userImageDal.Add(userImage);
                    return new SuccessDataResult<string>(result.Data, result.Message);
                }

            }
            return new ErrorDataResult<string>(null, Message.NotConvert);
        }

        public IResult Delete(UserImage userImage)
        {
            var oldpath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\wwwroot")) + _userImageDal.Get(p => p.UserId == userImage.UserId).ImagePath;
            IResult result = BusinessRules.Run(FileHelper.DeleteAsync(oldpath));

            if (result != null)
            {
                return result;
            }

            _userImageDal.Delete(userImage);
            return new SuccessResult(Message.PostDeleted);
        }

        [CacheAspect]
        [PerformanceAspect(5)]
        public IDataResult<List<UserImage>> GetAll(Expression<Func<UserImage, bool>> filter = null)
        {
            return new SuccessDataResult<List<UserImage>>(_userImageDal.GetAll(filter));
        }

        public IDataResult<UserImage> GetById(int id)
        {

            return new SuccessDataResult<UserImage>(_userImageDal.Get(I => I.UserId == id));
        }

        public IResult ProfileImageAdd(IFormFile file, UserImage userImage)
        {
            userImage.ProfileImage = FileHelper.AddAsync(file);
            userImage.UserImageDate = DateTime.Now;
            _userImageDal.ProfileImageAdd(userImage);
            return new SuccessResult(Message.PostAdded);
        }

        public IResult ProfileImageDelete(UserImage userImage)
        {
            var oldpath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\wwwroot")) + _userImageDal.Get(p => p.UserId == userImage.UserId).ProfileImage;
            IResult result = BusinessRules.Run(FileHelper.DeleteAsync(oldpath));

            if (result != null)
            {
                return result;
            }

            _userImageDal.Delete(userImage);
            return new SuccessResult(Message.PostDeleted);
        }
        public IDataResult<UserImage> Get(int id)
        {
            return new SuccessDataResult<UserImage>(_userImageDal.Get(p => p.UserImageId == id));
        }



        [CacheRemoveAspect("IUserImageService.Get")]
       
        public IResult Update(IFormFile file, UserImage userImage)
        {
            var oldpath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\wwwroot")) + _userImageDal.Get(p => p.UserId == userImage.UserId).ImagePath;
            userImage.ImagePath = FileHelper.UpdateAsync(oldpath, file);
            userImage.UserImageDate = DateTime.Now;
            _userImageDal.Update(userImage);
            return new SuccessResult(Message.PostUpdated);
        }
        private IDataResult<string> Convert(string url, string donusturulecekTur, string uniqueString)
        {
             donusturulecekTur= donusturulecekTur.ToLower();
            using (Converter converter = new Converter(url))
            {
                

                 if (donusturulecekTur == "jpg")
                {
                    ImageConvertOptions options = new ImageConvertOptions
                    {
                        Format = ImageFileType.Jpeg
                    };
                    converter.Convert(@"wwwroot/Image/" + uniqueString + ".jpg", options);
                    System.IO.File.Delete(url);
                    return new SuccessDataResult<string>(Server + "Image/" + uniqueString + ".jpg", Message.Convert);
                }

                else if (donusturulecekTur == "jpeg")
                {
                    ImageConvertOptions options = new ImageConvertOptions
                    {
                        Format = ImageFileType.Jpeg
                    };
                    converter.Convert(@"wwwroot/Image/" + uniqueString + ".jpeg", options);
                    System.IO.File.Delete(url);
                    return new SuccessDataResult<string>(Server + "Image/" + uniqueString + ".jpeg", Message.Convert);
                }
                else if (donusturulecekTur == ImageFileType.Png.ToString().ToLower())
                {
                    ImageConvertOptions options = new ImageConvertOptions
                    {
                        Format = ImageFileType.Png
                    };
                    converter.Convert(@"wwwroot/Image/" + uniqueString + ".png", options);
                    System.IO.File.Delete(url);
                    return new SuccessDataResult<string>(Server + "Image/" + uniqueString + ".png", Message.Convert);
                }

                else if (donusturulecekTur == ImageFileType.Webp.ToString().ToLower())
                {
                    ImageConvertOptions options = new ImageConvertOptions
                    {
                        Format = ImageFileType.Webp
                    };
                    converter.Convert(@"wwwroot/Image/" + uniqueString + ".webp", options);
                    System.IO.File.Delete(url);
                    return new SuccessDataResult<string>(Server + "Image/" + uniqueString + ".webp", Message.Convert);
                }
                return new ErrorDataResult<string>(Message.NotConvert);
            }
        }

        }
}
