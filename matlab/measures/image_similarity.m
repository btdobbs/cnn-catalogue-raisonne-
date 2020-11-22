tic;
image_path = '/users/btdobbs/images';
imds = imageDatastore({image_path},'IncludeSubfolders',true,'LabelSource','foldernames');
artists = unique(imds.Labels);
artist_count = height(artists);
measures = zeros(artist_count,4);
parfor artistIndex = 1:artist_count
    artist = char(artists(artistIndex));
    tic;
    artist_image_path = strcat(image_path, '/', artist);
    imds = imageDatastore({artist_image_path},'IncludeSubfolders',true,'LabelSource','foldernames');
    augimds = augmentedImageDatastore([224 224 3],imds);
    imageData = augimds.readall();
    ssim_val = 0;
    immse_val = 0;
    psnr_val = 0;
    image_count = height(imageData);
    for sourceIndex = 1:image_count
        for targetIndex = 1:image_count
            if sourceIndex < targetIndex
                imageSource = imageData(sourceIndex,1).input{:};
                imageTarget = imageData(targetIndex,1).input{:};
                ssim_val = ssim_val + ssim(imageSource,imageTarget);
                immse_val = immse_val + immse(imageSource,imageTarget);
                psnr_val = psnr_val + psnr(imageSource,imageTarget);
            end
        end
    end
    comparision_count = nchoosek(image_count,2);
    artist_measures = zeros(1,4);
    artist_measures(1,1) = artistIndex;
    artist_measures(1,2) = ssim_val/comparision_count;
    artist_measures(1,3) = immse_val/comparision_count;
    artist_measures(1,4) = psnr_val/comparision_count;
    measures(artistIndex, :) = artist_measures;
    current_task = getCurrentTask();
    disp(current_task.ID);
    disp(strcat(artist));
    toc;
end
save('artists.mat', 'artists');
save('measures.mat', 'measures');
toc;
